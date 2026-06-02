import { useState, useEffect } from 'react';
import request from '../services/api';

interface KanbanColumn {
  id: string;
  name: string;
  position: number;
}

interface Card {
  id: string;
  title: string;
  description?: string;
  columnId: string;
  priority: number;
  createdAt: string;
}

interface BoardViewProps {
  boardId: string;
  onBack: () => void;
}

export function BoardView({ boardId, onBack }: BoardViewProps) {
  const [columns, setColumns] = useState<KanbanColumn[]>([]);
  const [cards, setCards] = useState<Map<string, Card[]>>(new Map());
  const [loading, setLoading] = useState(true);
  const [draggedCard, setDraggedCard] = useState<Card | null>(null);

  useEffect(() => {
    loadBoardData();
  }, [boardId]);

  const loadBoardData = async () => {
    setLoading(true);
    const colResponse = await request<KanbanColumn[]>(`/boards/${boardId}/columns`);
    if (colResponse.data) {
      setColumns(colResponse.data.sort((a, b) => a.position - b.position));

      const newCards = new Map<string, Card[]>();
      for (const col of colResponse.data) {
        const cardResponse = await request<Card[]>(`/columns/${col.id}/cards`);
        newCards.set(col.id, cardResponse.data || []);
      }
      setCards(newCards);
    }
    setLoading(false);
  };

  const handleDragStart = (card: Card) => {
    setDraggedCard(card);
  };

  const handleDragOver = (e: React.DragEvent) => {
    e.preventDefault();
  };

  const handleDrop = async (columnId: string) => {
    if (!draggedCard || draggedCard.columnId === columnId) {
      setDraggedCard(null);
      return;
    }

    const updateResponse = await request(`/cards/${draggedCard.id}`, 'PUT', {
      columnId,
    });

    if (!updateResponse.error) {
      const oldCards = cards.get(draggedCard.columnId) || [];
      const newCards = cards.get(columnId) || [];

      setCards(
        new Map([
          ...cards,
          [draggedCard.columnId, oldCards.filter((c) => c.id !== draggedCard.id)],
          [columnId, [...newCards, { ...draggedCard, columnId }]],
        ])
      );
    }

    setDraggedCard(null);
  };

  if (loading) {
    return (
      <div className="min-h-screen flex items-center justify-center bg-gradient-to-br from-gray-950 via-gray-900 to-gray-950">
        <div className="animate-spin rounded-full h-8 w-8 border-b-2 border-indigo-600"></div>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-gradient-to-br from-gray-950 via-gray-900 to-gray-950">
      <header className="backdrop-blur-xl bg-white/5 border-b border-white/10 sticky top-0">
        <div className="max-w-7xl mx-auto px-6 py-4 flex items-center gap-4">
          <button
            onClick={onBack}
            className="px-3 py-2 rounded-lg bg-white/5 hover:bg-white/10 border border-white/10 text-gray-300 transition"
          >
            ← Voltar
          </button>
          <h1 className="text-2xl font-bold text-white">Board</h1>
        </div>
      </header>

      <main className="p-6 overflow-x-auto">
        <div className="flex gap-6 min-w-max">
          {columns.map((column) => (
            <div
              key={column.id}
              className="w-80 backdrop-blur-xl bg-white/5 border border-white/10 rounded-xl p-4"
            >
              <h2 className="font-bold text-white mb-4 text-lg">{column.name}</h2>

              <div
                onDragOver={handleDragOver}
                onDrop={() => handleDrop(column.id)}
                className="space-y-3 min-h-96"
              >
                {(cards.get(column.id) || []).map((card) => (
                  <div
                    key={card.id}
                    draggable
                    onDragStart={() => handleDragStart(card)}
                    className="p-3 bg-indigo-600/20 border border-indigo-500/30 rounded-lg cursor-move hover:bg-indigo-600/30 transition"
                  >
                    <p className="text-white font-medium text-sm">{card.title}</p>
                    {card.description && (
                      <p className="text-gray-400 text-xs mt-1 line-clamp-2">{card.description}</p>
                    )}
                    <div className="flex justify-between items-center mt-2">
                      <span className="text-xs text-gray-500">
                        Prioridade: {card.priority}
                      </span>
                    </div>
                  </div>
                ))}
              </div>
            </div>
          ))}
        </div>
      </main>
    </div>
  );
}

