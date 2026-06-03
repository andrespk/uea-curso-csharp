import { useEffect, useMemo, useState } from 'react';
import { authService, Board, boardService, User, userService } from '../services/api';

interface DashboardProps {
  onBoardSelect: (boardId: string) => void;
  onLogout: () => void;
}

export function Dashboard({ onBoardSelect, onLogout }: DashboardProps) {
  const [boards, setBoards] = useState<Board[]>([]);
  const [ownedBoards, setOwnedBoards] = useState<Board[]>([]);
  const [users, setUsers] = useState<User[]>([]);
  const [me, setMe] = useState<User | null>(null);
  const [name, setName] = useState('');
  const [description, setDescription] = useState('');
  const [editingBoardId, setEditingBoardId] = useState<string | null>(null);
  const [filter, setFilter] = useState('');
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    loadDashboard();
  }, []);

  const filteredBoards = useMemo(() => {
    const term = filter.trim().toLowerCase();
    if (!term) return boards;
    return boards.filter(
      (board) =>
        board.name.toLowerCase().includes(term) ||
        (board.description || '').toLowerCase().includes(term)
    );
  }, [boards, filter]);

  const loadDashboard = async () => {
    setLoading(true);
    setError('');

    const [meResponse, boardsResponse, usersResponse] = await Promise.all([
      authService.me(),
      boardService.list(),
      userService.list(),
    ]);

    if (meResponse.error) {
      setError(meResponse.error);
    } else {
      setMe(meResponse.data || null);
      if (meResponse.data?.id) {
        const ownedResponse = await boardService.listOwnedBy(meResponse.data.id);
        setOwnedBoards(ownedResponse.data || []);
      }
    }

    if (boardsResponse.error) setError(boardsResponse.error);
    setBoards(boardsResponse.data || []);
    setUsers(usersResponse.data || []);
    setLoading(false);
  };

  const resetForm = () => {
    setName('');
    setDescription('');
    setEditingBoardId(null);
  };

  const saveBoard = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');

    const response = editingBoardId
      ? await boardService.update(editingBoardId, { name, description })
      : await boardService.create({ name, description });

    if (response.error) {
      setError(response.error);
      return;
    }

    resetForm();
    await loadDashboard();
  };

  const startEdit = (board: Board) => {
    setEditingBoardId(board.id);
    setName(board.name);
    setDescription(board.description || '');
  };

  const deleteBoard = async (board: Board) => {
    if (!window.confirm(`Remover o board "${board.name}"?`)) return;
    const response = await boardService.remove(board.id);
    if (response.error) {
      setError(response.error);
      return;
    }
    await loadDashboard();
  };

  return (
    <main className="min-h-screen bg-slate-100 text-slate-950">
      <header className="border-b border-slate-200 bg-white">
        <div className="mx-auto flex max-w-7xl items-center justify-between px-6 py-4">
          <div>
            <p className="text-xs font-semibold uppercase tracking-wider text-blue-700">
              MiniKanban
            </p>
            <h1 className="text-xl font-bold text-slate-950">Painel de boards</h1>
          </div>

          <div className="flex items-center gap-3">
            {me && (
              <div className="hidden text-right text-sm sm:block">
                <p className="font-semibold text-slate-900">{me.name || me.username}</p>
                <p className="text-slate-500">{me.email}</p>
              </div>
            )}
            <button
              onClick={onLogout}
              className="rounded border border-slate-300 bg-white px-4 py-2 text-sm font-semibold text-slate-700 hover:bg-slate-50"
            >
              Sair
            </button>
          </div>
        </div>
      </header>

      <div className="mx-auto grid max-w-7xl grid-cols-1 gap-6 px-6 py-6 lg:grid-cols-[320px_1fr]">
        <aside className="space-y-4">
          <section className="rounded border border-slate-200 bg-white p-4">
            <h2 className="text-base font-bold text-slate-950">
              {editingBoardId ? 'Editar board' : 'Novo board'}
            </h2>
            <form onSubmit={saveBoard} className="mt-4 space-y-3">
              <Input label="Nome" value={name} onChange={setName} required />
              <Textarea label="Descricao" value={description} onChange={setDescription} />
              <div className="flex gap-2">
                <button
                  type="submit"
                  className="h-10 flex-1 rounded bg-blue-700 px-3 text-sm font-semibold text-white hover:bg-blue-800"
                >
                  {editingBoardId ? 'Salvar' : 'Criar'}
                </button>
                {editingBoardId && (
                  <button
                    type="button"
                    onClick={resetForm}
                    className="h-10 rounded border border-slate-300 px-3 text-sm font-semibold text-slate-700 hover:bg-slate-50"
                  >
                    Cancelar
                  </button>
                )}
              </div>
            </form>
          </section>

          <section className="rounded border border-slate-200 bg-white p-4">
            <h2 className="text-base font-bold text-slate-950">Resumo</h2>
            <div className="mt-4 grid grid-cols-3 gap-2 text-center">
              <Metric value={boards.length} label="Boards" />
              <Metric value={ownedBoards.length} label="Criados" />
              <Metric value={users.length} label="Usuarios" />
            </div>
          </section>

          <section className="rounded border border-slate-200 bg-white p-4">
            <h2 className="text-base font-bold text-slate-950">Usuarios</h2>
            <div className="mt-3 max-h-64 space-y-2 overflow-auto">
              {users.map((user) => (
                <div key={user.id} className="rounded border border-slate-200 p-2 text-sm">
                  <p className="font-semibold text-slate-800">{user.name || user.username}</p>
                  <p className="truncate text-xs text-slate-500">{user.email}</p>
                </div>
              ))}
            </div>
          </section>
        </aside>

        <section className="rounded border border-slate-200 bg-white p-4">
          <div className="flex flex-col gap-3 border-b border-slate-200 pb-4 md:flex-row md:items-center md:justify-between">
            <div>
              <h2 className="text-lg font-bold text-slate-950">Boards disponiveis</h2>
              <p className="mt-1 text-sm text-slate-500">
                Entre em um board para gerenciar colunas, cards, tags, membros e comentarios.
              </p>
            </div>
            <input
              value={filter}
              onChange={(e) => setFilter(e.target.value)}
              placeholder="Filtrar boards"
              className="h-10 w-full rounded border border-slate-300 px-3 text-sm outline-none focus:border-blue-600 md:w-72"
            />
          </div>

          {loading && <p className="py-10 text-center text-sm text-slate-500">Carregando...</p>}

          {error && (
            <div className="mt-4 rounded border border-red-200 bg-red-50 p-3 text-sm text-red-700">
              {error}
            </div>
          )}

          {!loading && filteredBoards.length === 0 && (
            <div className="py-10 text-center">
              <p className="font-semibold text-slate-800">Nenhum board encontrado</p>
              <p className="mt-1 text-sm text-slate-500">Crie um board para iniciar o fluxo.</p>
            </div>
          )}

          <div className="mt-4 grid grid-cols-1 gap-3 xl:grid-cols-2">
            {filteredBoards.map((board) => (
              <article key={board.id} className="rounded border border-slate-200 p-4">
                <div className="flex items-start justify-between gap-3">
                  <div>
                    <h3 className="text-base font-bold text-slate-950">{board.name}</h3>
                    <p className="mt-1 min-h-10 text-sm text-slate-600">
                      {board.description || 'Sem descricao.'}
                    </p>
                  </div>
                  {me?.id === board.ownerId && (
                    <span className="rounded bg-blue-50 px-2 py-1 text-xs font-semibold text-blue-700">
                      Owner
                    </span>
                  )}
                </div>
                <p className="mt-3 text-xs text-slate-500">
                  Criado em {new Date(board.createdAt).toLocaleDateString('pt-BR')}
                </p>
                <div className="mt-4 flex flex-wrap gap-2">
                  <button
                    onClick={() => onBoardSelect(board.id)}
                    className="rounded bg-slate-950 px-3 py-2 text-sm font-semibold text-white hover:bg-slate-800"
                  >
                    Abrir
                  </button>
                  <button
                    onClick={() => startEdit(board)}
                    className="rounded border border-slate-300 px-3 py-2 text-sm font-semibold text-slate-700 hover:bg-slate-50"
                  >
                    Editar
                  </button>
                  <button
                    onClick={() => deleteBoard(board)}
                    className="rounded border border-red-200 px-3 py-2 text-sm font-semibold text-red-700 hover:bg-red-50"
                  >
                    Remover
                  </button>
                </div>
              </article>
            ))}
          </div>
        </section>
      </div>
    </main>
  );
}

function Metric({ value, label }: { value: number; label: string }) {
  return (
    <div className="rounded border border-slate-200 bg-slate-50 p-2">
      <p className="text-lg font-bold text-slate-950">{value}</p>
      <p className="text-xs text-slate-500">{label}</p>
    </div>
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
        className="h-10 w-full rounded border border-slate-300 px-3 text-sm outline-none focus:border-blue-600 focus:ring-2 focus:ring-blue-100"
      />
    </label>
  );
}

function Textarea({
  label,
  value,
  onChange,
}: {
  label: string;
  value: string;
  onChange: (value: string) => void;
}) {
  return (
    <label className="block">
      <span className="mb-1 block text-sm font-medium text-slate-700">{label}</span>
      <textarea
        value={value}
        onChange={(e) => onChange(e.target.value)}
        rows={3}
        className="w-full rounded border border-slate-300 px-3 py-2 text-sm outline-none focus:border-blue-600 focus:ring-2 focus:ring-blue-100"
      />
    </label>
  );
}
