import { useEffect, useMemo, useState } from 'react';
import {
  Board,
  BoardMember,
  boardMemberService,
  boardService,
  Card,
  cardService,
  CardTag,
  cardTagService,
  columnService,
  Comment,
  commentService,
  KanbanColumn,
  Tag,
  tagService,
  User,
  userService,
} from '../services/api';

interface BoardViewProps {
  boardId: string;
  onBack: () => void;
}

type Tab = 'kanban' | 'members' | 'tags';

const roleOptions = [
  { label: 'Admin', value: '1' },
  { label: 'Member', value: '2' },
  { label: 'Viewer', value: '3' },
];

const roleLabels: Record<string, string> = {
  '0': 'Owner',
  '1': 'Admin',
  '2': 'Member',
  '3': 'Viewer',
  Owner: 'Owner',
  Admin: 'Admin',
  Member: 'Member',
  Viewer: 'Viewer',
};

export function BoardView({ boardId, onBack }: BoardViewProps) {
  const [board, setBoard] = useState<Board | null>(null);
  const [columns, setColumns] = useState<KanbanColumn[]>([]);
  const [cardsByColumn, setCardsByColumn] = useState<Map<string, Card[]>>(new Map());
  const [members, setMembers] = useState<BoardMember[]>([]);
  const [users, setUsers] = useState<User[]>([]);
  const [tags, setTags] = useState<Tag[]>([]);
  const [cardTags, setCardTags] = useState<Map<string, CardTag[]>>(new Map());
  const [comments, setComments] = useState<Map<string, Comment[]>>(new Map());
  const [selectedCard, setSelectedCard] = useState<Card | null>(null);
  const [activeTab, setActiveTab] = useState<Tab>('kanban');
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(true);

  const [columnName, setColumnName] = useState('');
  const [columnOrder, setColumnOrder] = useState(1);
  const [columnWipLimit, setColumnWipLimit] = useState(5);

  const [cardColumnId, setCardColumnId] = useState('');
  const [cardTitle, setCardTitle] = useState('');
  const [cardDescription, setCardDescription] = useState('');
  const [cardPriority, setCardPriority] = useState(2);
  const [cardAssignedToUserId, setCardAssignedToUserId] = useState('');
  const [cardDueDate, setCardDueDate] = useState('');

  const [tagName, setTagName] = useState('');
  const [tagColor, setTagColor] = useState('#2563eb');
  const [memberUserId, setMemberUserId] = useState('');
  const [memberRole, setMemberRole] = useState('2');
  const [commentText, setCommentText] = useState('');
  const [selectedTagId, setSelectedTagId] = useState('');

  useEffect(() => {
    loadBoardData();
  }, [boardId]);

  const allCards = useMemo(
    () => Array.from(cardsByColumn.values()).flat(),
    [cardsByColumn]
  );

  const memberUsers = useMemo(() => {
    const ids = new Set(members.map((member) => member.userId));
    return users.filter((user) => ids.has(user.id));
  }, [members, users]);

  const availableUsers = useMemo(() => {
    const ids = new Set(members.map((member) => member.userId));
    return users.filter((user) => !ids.has(user.id));
  }, [members, users]);

  const loadBoardData = async () => {
    setLoading(true);
    setError('');

    const [boardResponse, columnsResponse, membersResponse, usersResponse, tagsResponse] =
      await Promise.all([
        boardService.get(boardId),
        columnService.list(boardId),
        boardMemberService.list(boardId),
        userService.list(),
        tagService.list(boardId),
      ]);

    if (boardResponse.error) setError(boardResponse.error);
    if (columnsResponse.error) setError(columnsResponse.error);

    const loadedColumns = (columnsResponse.data || []).sort((a, b) => a.order - b.order);
    setBoard(boardResponse.data || null);
    setColumns(loadedColumns);
    setMembers(membersResponse.data || []);
    setUsers(usersResponse.data || []);
    setTags(tagsResponse.data || []);

    if (loadedColumns.length > 0 && !cardColumnId) {
      setCardColumnId(loadedColumns[0].id);
    }

    const nextCards = new Map<string, Card[]>();
    const nextCardTags = new Map<string, CardTag[]>();
    const nextComments = new Map<string, Comment[]>();

    for (const column of loadedColumns) {
      const cardResponse = await cardService.listByColumn(column.id);
      const cards = cardResponse.data || [];
      nextCards.set(column.id, cards);

      for (const card of cards) {
        const [tagResponse, commentResponse] = await Promise.all([
          cardTagService.list(card.id),
          commentService.list(card.id),
        ]);
        nextCardTags.set(card.id, tagResponse.data || []);
        nextComments.set(card.id, commentResponse.data || []);
      }
    }

    setCardsByColumn(nextCards);
    setCardTags(nextCardTags);
    setComments(nextComments);
    setLoading(false);
  };

  const createColumn = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');
    const response = await columnService.create({
      boardId,
      name: columnName,
      order: columnOrder,
      wipLimit: columnWipLimit,
    });
    if (response.error) {
      setError(response.error);
      return;
    }
    setColumnName('');
    setColumnOrder(columnOrder + 1);
    await loadBoardData();
  };

  const updateColumn = async (column: KanbanColumn) => {
    const name = window.prompt('Novo nome da coluna', column.name);
    if (!name) return;
    const response = await columnService.update(column.id, { name });
    if (response.error) setError(response.error);
    await loadBoardData();
  };

  const deleteColumn = async (column: KanbanColumn) => {
    if (!window.confirm(`Remover a coluna "${column.name}"?`)) return;
    const response = await columnService.remove(column.id);
    if (response.error) setError(response.error);
    await loadBoardData();
  };

  const createCard = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');
    const response = await cardService.create({
      columnId: cardColumnId,
      assignedToUserId: cardAssignedToUserId || undefined,
      title: cardTitle,
      description: cardDescription,
      priority: cardPriority,
      dueDate: cardDueDate ? new Date(cardDueDate).toISOString() : undefined,
    });
    if (response.error) {
      setError(response.error);
      return;
    }
    setCardTitle('');
    setCardDescription('');
    setCardDueDate('');
    await loadBoardData();
  };

  const moveCard = async (card: Card, columnId: string) => {
    const response = await cardService.update(card.id, { columnId });
    if (response.error) {
      setError(response.error);
      return;
    }
    await loadBoardData();
  };

  const updateCardTitle = async (card: Card) => {
    const title = window.prompt('Novo titulo do card', card.title);
    if (!title) return;
    const response = await cardService.update(card.id, { title });
    if (response.error) setError(response.error);
    await loadBoardData();
  };

  const deleteCard = async (card: Card) => {
    if (!window.confirm(`Remover o card "${card.title}"?`)) return;
    const response = await cardService.remove(card.id);
    if (response.error) setError(response.error);
    if (selectedCard?.id === card.id) setSelectedCard(null);
    await loadBoardData();
  };

  const createTag = async (e: React.FormEvent) => {
    e.preventDefault();
    const response = await tagService.create({ boardId, name: tagName, color: tagColor });
    if (response.error) {
      setError(response.error);
      return;
    }
    setTagName('');
    await loadBoardData();
  };

  const updateTag = async (tag: Tag) => {
    const name = window.prompt('Novo nome da tag', tag.name);
    if (!name) return;
    const response = await tagService.update(tag.id, { name, color: tag.color });
    if (response.error) setError(response.error);
    await loadBoardData();
  };

  const deleteTag = async (tag: Tag) => {
    if (!window.confirm(`Remover a tag "${tag.name}"?`)) return;
    const response = await tagService.remove(tag.id);
    if (response.error) setError(response.error);
    await loadBoardData();
  };

  const addMember = async (e: React.FormEvent) => {
    e.preventDefault();
    const response = await boardMemberService.add({
      boardId,
      userId: memberUserId,
      role: memberRole,
    });
    if (response.error) {
      setError(response.error);
      return;
    }
    setMemberUserId('');
    await loadBoardData();
  };

  const updateMemberRole = async (member: BoardMember, role: string) => {
    const response = await boardMemberService.update(member.id, role);
    if (response.error) setError(response.error);
    await loadBoardData();
  };

  const removeMember = async (member: BoardMember) => {
    if (!window.confirm('Remover membro do board?')) return;
    const response = await boardMemberService.remove(member.id);
    if (response.error) setError(response.error);
    await loadBoardData();
  };

  const addTagToSelectedCard = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!selectedCard || !selectedTagId) return;
    const response = await cardTagService.add({ cardId: selectedCard.id, tagId: selectedTagId });
    if (response.error) {
      setError(response.error);
      return;
    }
    setSelectedTagId('');
    await loadBoardData();
  };

  const removeTagFromCard = async (cardId: string, tagId: string) => {
    const response = await cardTagService.remove(cardId, tagId);
    if (response.error) setError(response.error);
    await loadBoardData();
  };

  const createComment = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!selectedCard) return;
    const response = await commentService.create({ cardId: selectedCard.id, content: commentText });
    if (response.error) {
      setError(response.error);
      return;
    }
    setCommentText('');
    await loadBoardData();
  };

  const updateComment = async (comment: Comment) => {
    const content = window.prompt('Novo comentario', comment.content);
    if (!content) return;
    const response = await commentService.update(comment.id, content);
    if (response.error) setError(response.error);
    await loadBoardData();
  };

  const deleteComment = async (comment: Comment) => {
    const response = await commentService.remove(comment.id);
    if (response.error) setError(response.error);
    await loadBoardData();
  };

  const getUserName = (id?: string) =>
    users.find((user) => user.id === id)?.name || users.find((user) => user.id === id)?.username || 'Nao atribuido';

  const getTagById = (id: string) => tags.find((tag) => tag.id === id);

  if (loading) {
    return (
      <main className="flex min-h-screen items-center justify-center bg-slate-100 text-slate-950">
        <p className="text-sm font-medium text-slate-600">Carregando board...</p>
      </main>
    );
  }

  return (
    <main className="min-h-screen bg-slate-100 text-slate-950">
      <header className="border-b border-slate-200 bg-white">
        <div className="mx-auto flex max-w-[1500px] flex-col gap-3 px-6 py-4 lg:flex-row lg:items-center lg:justify-between">
          <div className="flex items-center gap-3">
            <button
              onClick={onBack}
              className="rounded border border-slate-300 bg-white px-3 py-2 text-sm font-semibold text-slate-700 hover:bg-slate-50"
            >
              Voltar
            </button>
            <div>
              <h1 className="text-xl font-bold text-slate-950">{board?.name || 'Board'}</h1>
              <p className="text-sm text-slate-500">{board?.description || 'Sem descricao.'}</p>
            </div>
          </div>

          <nav className="flex rounded border border-slate-200 bg-slate-50 p-1">
            {(['kanban', 'members', 'tags'] as Tab[]).map((tab) => (
              <button
                key={tab}
                onClick={() => setActiveTab(tab)}
                className={`rounded px-3 py-2 text-sm font-semibold ${
                  activeTab === tab ? 'bg-white text-slate-950 shadow-sm' : 'text-slate-600'
                }`}
              >
                {tab === 'kanban' ? 'Kanban' : tab === 'members' ? 'Membros' : 'Tags'}
              </button>
            ))}
          </nav>
        </div>
      </header>

      {error && (
        <div className="mx-auto mt-4 max-w-[1500px] px-6">
          <div className="rounded border border-red-200 bg-red-50 p-3 text-sm text-red-700">
            {error}
          </div>
        </div>
      )}

      {activeTab === 'kanban' && (
        <div className="mx-auto grid max-w-[1500px] grid-cols-1 gap-6 px-6 py-6 xl:grid-cols-[320px_1fr_360px]">
          <aside className="space-y-4">
            <Panel title="Nova coluna">
              <form onSubmit={createColumn} className="space-y-3">
                <Input label="Nome" value={columnName} onChange={setColumnName} required />
                <div className="grid grid-cols-2 gap-2">
                  <NumberInput label="Ordem" value={columnOrder} onChange={setColumnOrder} />
                  <NumberInput label="WIP" value={columnWipLimit} onChange={setColumnWipLimit} />
                </div>
                <PrimaryButton>Criar coluna</PrimaryButton>
              </form>
            </Panel>

            <Panel title="Novo card">
              <form onSubmit={createCard} className="space-y-3">
                <Select label="Coluna" value={cardColumnId} onChange={setCardColumnId}>
                  {columns.map((column) => (
                    <option key={column.id} value={column.id}>
                      {column.name}
                    </option>
                  ))}
                </Select>
                <Input label="Titulo" value={cardTitle} onChange={setCardTitle} required />
                <Textarea label="Descricao" value={cardDescription} onChange={setCardDescription} />
                <Select label="Responsavel" value={cardAssignedToUserId} onChange={setCardAssignedToUserId}>
                  <option value="">Sem responsavel</option>
                  {memberUsers.map((user) => (
                    <option key={user.id} value={user.id}>
                      {user.name || user.username}
                    </option>
                  ))}
                </Select>
                <div className="grid grid-cols-2 gap-2">
                  <NumberInput label="Prioridade" value={cardPriority} onChange={setCardPriority} />
                  <DateInput label="Prazo" value={cardDueDate} onChange={setCardDueDate} />
                </div>
                <PrimaryButton>Criar card</PrimaryButton>
              </form>
            </Panel>
          </aside>

          <section className="overflow-x-auto">
            <div className="flex min-h-[680px] gap-4">
              {columns.length === 0 && (
                <div className="flex flex-1 items-center justify-center rounded border border-dashed border-slate-300 bg-white p-8 text-center">
                  <div>
                    <p className="font-semibold text-slate-800">Nenhuma coluna criada</p>
                    <p className="mt-1 text-sm text-slate-500">Crie uma coluna para comecar o board.</p>
                  </div>
                </div>
              )}

              {columns.map((column) => (
                <section key={column.id} className="w-80 shrink-0 rounded border border-slate-200 bg-white">
                  <div className="border-b border-slate-200 p-3">
                    <div className="flex items-start justify-between gap-2">
                      <div>
                        <h2 className="font-bold text-slate-950">{column.name}</h2>
                        <p className="mt-1 text-xs text-slate-500">
                          Ordem {column.order} {column.wipLimit ? `• WIP ${column.wipLimit}` : ''}
                        </p>
                      </div>
                      <MenuButtons
                        onEdit={() => updateColumn(column)}
                        onDelete={() => deleteColumn(column)}
                      />
                    </div>
                  </div>

                  <div className="space-y-3 p-3">
                    {(cardsByColumn.get(column.id) || []).map((card) => (
                      <article
                        key={card.id}
                        className={`rounded border p-3 ${
                          selectedCard?.id === card.id
                            ? 'border-blue-500 bg-blue-50'
                            : 'border-slate-200 bg-slate-50'
                        }`}
                      >
                        <button
                          onClick={() => setSelectedCard(card)}
                          className="block w-full text-left"
                        >
                          <h3 className="font-semibold text-slate-950">{card.title}</h3>
                          {card.description && (
                            <p className="mt-1 line-clamp-2 text-sm text-slate-600">{card.description}</p>
                          )}
                        </button>

                        <div className="mt-3 flex flex-wrap gap-1">
                          {(cardTags.get(card.id) || []).map((link) => {
                            const tag = getTagById(link.tagId);
                            if (!tag) return null;
                            return (
                              <span
                                key={link.tagId}
                                className="rounded px-2 py-1 text-xs font-semibold text-white"
                                style={{ backgroundColor: tag.color || '#475569' }}
                              >
                                {tag.name}
                              </span>
                            );
                          })}
                        </div>

                        <div className="mt-3 text-xs text-slate-500">
                          <p>Prioridade {card.priority}</p>
                          <p>Responsavel: {getUserName(card.assignedToUserId)}</p>
                        </div>

                        <div className="mt-3 flex flex-wrap gap-2">
                          <select
                            value={card.columnId}
                            onChange={(e) => moveCard(card, e.target.value)}
                            className="h-8 rounded border border-slate-300 bg-white px-2 text-xs"
                          >
                            {columns.map((target) => (
                              <option key={target.id} value={target.id}>
                                {target.name}
                              </option>
                            ))}
                          </select>
                          <SmallButton onClick={() => updateCardTitle(card)}>Editar</SmallButton>
                          <DangerSmallButton onClick={() => deleteCard(card)}>Remover</DangerSmallButton>
                        </div>
                      </article>
                    ))}
                  </div>
                </section>
              ))}
            </div>
          </section>

          <CardDetails
            card={selectedCard}
            tags={tags}
            cardTags={selectedCard ? cardTags.get(selectedCard.id) || [] : []}
            comments={selectedCard ? comments.get(selectedCard.id) || [] : []}
            selectedTagId={selectedTagId}
            setSelectedTagId={setSelectedTagId}
            commentText={commentText}
            setCommentText={setCommentText}
            addTagToSelectedCard={addTagToSelectedCard}
            removeTagFromCard={removeTagFromCard}
            createComment={createComment}
            updateComment={updateComment}
            deleteComment={deleteComment}
            getTagById={getTagById}
            getUserName={getUserName}
          />
        </div>
      )}

      {activeTab === 'members' && (
        <div className="mx-auto grid max-w-5xl grid-cols-1 gap-6 px-6 py-6 lg:grid-cols-[320px_1fr]">
          <Panel title="Adicionar membro">
            <form onSubmit={addMember} className="space-y-3">
              <Select label="Usuario" value={memberUserId} onChange={setMemberUserId}>
                <option value="">Selecione</option>
                {availableUsers.map((user) => (
                  <option key={user.id} value={user.id}>
                    {user.name || user.username}
                  </option>
                ))}
              </Select>
              <Select label="Papel" value={memberRole} onChange={setMemberRole}>
                {roleOptions.map((role) => (
                  <option key={role.value} value={role.value}>
                    {role.label}
                  </option>
                ))}
              </Select>
              <PrimaryButton>Adicionar</PrimaryButton>
            </form>
          </Panel>

          <Panel title="Membros do board">
            <div className="space-y-2">
              {members.map((member) => (
                <div key={member.id} className="flex flex-col gap-2 rounded border border-slate-200 p-3 md:flex-row md:items-center md:justify-between">
                  <div>
                    <p className="font-semibold text-slate-900">{getUserName(member.userId)}</p>
                    <p className="text-xs text-slate-500">Entrou em {new Date(member.joinedAt).toLocaleDateString('pt-BR')}</p>
                  </div>
                  <div className="flex gap-2">
                    <select
                      value={String(member.role)}
                      onChange={(e) => updateMemberRole(member, e.target.value)}
                      className="h-9 rounded border border-slate-300 px-2 text-sm"
                      disabled={String(member.role) === 'Owner' || Number(member.role) === 0}
                    >
                      <option value="0">{roleLabels[String(member.role)] || 'Owner'}</option>
                      {roleOptions.map((role) => (
                        <option key={role.value} value={role.value}>
                          {role.label}
                        </option>
                      ))}
                    </select>
                    <DangerSmallButton onClick={() => removeMember(member)}>Remover</DangerSmallButton>
                  </div>
                </div>
              ))}
            </div>
          </Panel>
        </div>
      )}

      {activeTab === 'tags' && (
        <div className="mx-auto grid max-w-5xl grid-cols-1 gap-6 px-6 py-6 lg:grid-cols-[320px_1fr]">
          <Panel title="Nova tag">
            <form onSubmit={createTag} className="space-y-3">
              <Input label="Nome" value={tagName} onChange={setTagName} required />
              <ColorInput label="Cor" value={tagColor} onChange={setTagColor} />
              <PrimaryButton>Criar tag</PrimaryButton>
            </form>
          </Panel>

          <Panel title="Tags do board">
            <div className="grid grid-cols-1 gap-3 md:grid-cols-2">
              {tags.map((tag) => (
                <div key={tag.id} className="rounded border border-slate-200 p-3">
                  <div className="flex items-center gap-3">
                    <span className="h-6 w-6 rounded" style={{ backgroundColor: tag.color || '#475569' }} />
                    <div className="flex-1">
                      <p className="font-semibold text-slate-950">{tag.name}</p>
                      <p className="text-xs text-slate-500">{tag.color || 'Sem cor'}</p>
                    </div>
                  </div>
                  <div className="mt-3 flex gap-2">
                    <SmallButton onClick={() => updateTag(tag)}>Editar</SmallButton>
                    <DangerSmallButton onClick={() => deleteTag(tag)}>Remover</DangerSmallButton>
                  </div>
                </div>
              ))}
            </div>
          </Panel>
        </div>
      )}
    </main>
  );
}

function CardDetails(props: {
  card: Card | null;
  tags: Tag[];
  cardTags: CardTag[];
  comments: Comment[];
  selectedTagId: string;
  setSelectedTagId: (value: string) => void;
  commentText: string;
  setCommentText: (value: string) => void;
  addTagToSelectedCard: (e: React.FormEvent) => void;
  removeTagFromCard: (cardId: string, tagId: string) => void;
  createComment: (e: React.FormEvent) => void;
  updateComment: (comment: Comment) => void;
  deleteComment: (comment: Comment) => void;
  getTagById: (id: string) => Tag | undefined;
  getUserName: (id?: string) => string;
}) {
  if (!props.card) {
    return (
      <Panel title="Detalhes do card">
        <p className="text-sm text-slate-500">Selecione um card para gerenciar tags e comentarios.</p>
      </Panel>
    );
  }

  const linkedTagIds = new Set(props.cardTags.map((tag) => tag.tagId));
  const availableTags = props.tags.filter((tag) => !linkedTagIds.has(tag.id));

  return (
    <Panel title="Detalhes do card">
      <div className="space-y-5">
        <div>
          <h2 className="font-bold text-slate-950">{props.card.title}</h2>
          <p className="mt-1 text-sm text-slate-600">{props.card.description || 'Sem descricao.'}</p>
          <p className="mt-2 text-xs text-slate-500">
            Criado por {props.getUserName(props.card.createdByUserId)}
          </p>
        </div>

        <section>
          <h3 className="text-sm font-bold text-slate-900">Tags vinculadas</h3>
          <div className="mt-2 flex flex-wrap gap-2">
            {props.cardTags.map((link) => {
              const tag = props.getTagById(link.tagId);
              if (!tag) return null;
              return (
                <button
                  key={link.tagId}
                  onClick={() => props.removeTagFromCard(props.card!.id, link.tagId)}
                  className="rounded px-2 py-1 text-xs font-semibold text-white"
                  style={{ backgroundColor: tag.color || '#475569' }}
                  title="Clique para remover"
                >
                  {tag.name}
                </button>
              );
            })}
          </div>
          <form onSubmit={props.addTagToSelectedCard} className="mt-3 flex gap-2">
            <select
              value={props.selectedTagId}
              onChange={(e) => props.setSelectedTagId(e.target.value)}
              className="h-10 min-w-0 flex-1 rounded border border-slate-300 px-2 text-sm"
            >
              <option value="">Selecionar tag</option>
              {availableTags.map((tag) => (
                <option key={tag.id} value={tag.id}>
                  {tag.name}
                </option>
              ))}
            </select>
            <button className="rounded bg-slate-950 px-3 text-sm font-semibold text-white">
              Vincular
            </button>
          </form>
        </section>

        <section>
          <h3 className="text-sm font-bold text-slate-900">Comentarios</h3>
          <form onSubmit={props.createComment} className="mt-2 space-y-2">
            <textarea
              value={props.commentText}
              onChange={(e) => props.setCommentText(e.target.value)}
              rows={3}
              className="w-full rounded border border-slate-300 px-3 py-2 text-sm"
              placeholder="Adicionar comentario"
            />
            <PrimaryButton>Comentar</PrimaryButton>
          </form>
          <div className="mt-3 space-y-2">
            {props.comments.map((comment) => (
              <div key={comment.id} className="rounded border border-slate-200 p-3 text-sm">
                <p className="text-slate-700">{comment.content}</p>
                <p className="mt-1 text-xs text-slate-500">
                  {props.getUserName(comment.userId)} • {new Date(comment.createdAt).toLocaleString('pt-BR')}
                </p>
                <div className="mt-2 flex gap-2">
                  <SmallButton onClick={() => props.updateComment(comment)}>Editar</SmallButton>
                  <DangerSmallButton onClick={() => props.deleteComment(comment)}>Remover</DangerSmallButton>
                </div>
              </div>
            ))}
          </div>
        </section>
      </div>
    </Panel>
  );
}

function Panel({ title, children }: { title: string; children: React.ReactNode }) {
  return (
    <section className="rounded border border-slate-200 bg-white p-4">
      <h2 className="text-base font-bold text-slate-950">{title}</h2>
      <div className="mt-4">{children}</div>
    </section>
  );
}

function Input({
  label,
  value,
  onChange,
  required,
}: {
  label: string;
  value: string;
  onChange: (value: string) => void;
  required?: boolean;
}) {
  return (
    <label className="block">
      <span className="mb-1 block text-sm font-medium text-slate-700">{label}</span>
      <input
        value={value}
        required={required}
        onChange={(e) => onChange(e.target.value)}
        className="h-10 w-full rounded border border-slate-300 px-3 text-sm outline-none focus:border-blue-600"
      />
    </label>
  );
}

function Textarea({ label, value, onChange }: { label: string; value: string; onChange: (value: string) => void }) {
  return (
    <label className="block">
      <span className="mb-1 block text-sm font-medium text-slate-700">{label}</span>
      <textarea
        value={value}
        onChange={(e) => onChange(e.target.value)}
        rows={3}
        className="w-full rounded border border-slate-300 px-3 py-2 text-sm outline-none focus:border-blue-600"
      />
    </label>
  );
}

function NumberInput({ label, value, onChange }: { label: string; value: number; onChange: (value: number) => void }) {
  return (
    <label className="block">
      <span className="mb-1 block text-sm font-medium text-slate-700">{label}</span>
      <input
        type="number"
        value={value}
        onChange={(e) => onChange(Number(e.target.value))}
        className="h-10 w-full rounded border border-slate-300 px-3 text-sm outline-none focus:border-blue-600"
      />
    </label>
  );
}

function DateInput({ label, value, onChange }: { label: string; value: string; onChange: (value: string) => void }) {
  return (
    <label className="block">
      <span className="mb-1 block text-sm font-medium text-slate-700">{label}</span>
      <input
        type="datetime-local"
        value={value}
        onChange={(e) => onChange(e.target.value)}
        className="h-10 w-full rounded border border-slate-300 px-2 text-xs outline-none focus:border-blue-600"
      />
    </label>
  );
}

function ColorInput({ label, value, onChange }: { label: string; value: string; onChange: (value: string) => void }) {
  return (
    <label className="block">
      <span className="mb-1 block text-sm font-medium text-slate-700">{label}</span>
      <input
        type="color"
        value={value}
        onChange={(e) => onChange(e.target.value)}
        className="h-10 w-full rounded border border-slate-300 bg-white px-2"
      />
    </label>
  );
}

function Select({
  label,
  value,
  onChange,
  children,
}: {
  label: string;
  value: string;
  onChange: (value: string) => void;
  children: React.ReactNode;
}) {
  return (
    <label className="block">
      <span className="mb-1 block text-sm font-medium text-slate-700">{label}</span>
      <select
        value={value}
        onChange={(e) => onChange(e.target.value)}
        className="h-10 w-full rounded border border-slate-300 bg-white px-3 text-sm outline-none focus:border-blue-600"
      >
        {children}
      </select>
    </label>
  );
}

function PrimaryButton({ children }: { children: React.ReactNode }) {
  return (
    <button className="h-10 w-full rounded bg-blue-700 px-3 text-sm font-semibold text-white hover:bg-blue-800">
      {children}
    </button>
  );
}

function SmallButton({ children, onClick }: { children: React.ReactNode; onClick: () => void }) {
  return (
    <button
      type="button"
      onClick={onClick}
      className="rounded border border-slate-300 bg-white px-2 py-1 text-xs font-semibold text-slate-700 hover:bg-slate-50"
    >
      {children}
    </button>
  );
}

function DangerSmallButton({ children, onClick }: { children: React.ReactNode; onClick: () => void }) {
  return (
    <button
      type="button"
      onClick={onClick}
      className="rounded border border-red-200 bg-white px-2 py-1 text-xs font-semibold text-red-700 hover:bg-red-50"
    >
      {children}
    </button>
  );
}

function MenuButtons({ onEdit, onDelete }: { onEdit: () => void; onDelete: () => void }) {
  return (
    <div className="flex gap-1">
      <SmallButton onClick={onEdit}>Editar</SmallButton>
      <DangerSmallButton onClick={onDelete}>Remover</DangerSmallButton>
    </div>
  );
}
