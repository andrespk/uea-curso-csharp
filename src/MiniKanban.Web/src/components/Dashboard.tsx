import { useState, useEffect } from 'react';
import request from '../services/api';

interface Board {
  id: string;
  name: string;
  description?: string;
  createdAt: string;
}

interface DashboardProps {
  onBoardSelect: (boardId: string) => void;
  onLogout: () => void;
}

export function Dashboard({ onBoardSelect, onLogout }: DashboardProps) {
  const [boards, setBoards] = useState<Board[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    loadBoards();
  }, []);

  const loadBoards = async () => {
    setLoading(true);
    const response = await request<Board[]>('/boards');
    if (response.error) {
      setError(response.error);
    } else {
      setBoards(response.data || []);
    }
    setLoading(false);
  };

  return (
    <div className="min-h-screen bg-gradient-to-br from-gray-950 via-gray-900 to-gray-950">
      <header className="backdrop-blur-xl bg-white/5 border-b border-white/10 sticky top-0">
        <div className="max-w-7xl mx-auto px-6 py-4 flex justify-between items-center">
          <h1 className="text-3xl font-bold bg-gradient-to-r from-indigo-400 to-purple-400 bg-clip-text text-transparent">
            MiniKanban
          </h1>
          <button
            onClick={onLogout}
            className="px-4 py-2 rounded-lg bg-red-600/20 hover:bg-red-600/30 text-red-200 border border-red-500/30 transition"
          >
            Sair
          </button>
        </div>
      </header>

      <main className="max-w-7xl mx-auto px-6 py-12">
        <div className="mb-8">
          <h2 className="text-2xl font-bold text-white mb-2">Meus Boards</h2>
          <p className="text-gray-400">Selecione um board para começar</p>
        </div>

        {loading && (
          <div className="flex justify-center py-12">
            <div className="animate-spin rounded-full h-8 w-8 border-b-2 border-indigo-600"></div>
          </div>
        )}

        {error && (
          <div className="p-4 rounded-lg bg-red-500/20 border border-red-500/50 text-red-200">
            {error}
          </div>
        )}

        {!loading && boards.length === 0 && (
          <div className="text-center py-12">
            <p className="text-gray-400 text-lg">Nenhum board encontrado</p>
          </div>
        )}

        {!loading && boards.length > 0 && (
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
            {boards.map((board) => (
              <button
                key={board.id}
                onClick={() => onBoardSelect(board.id)}
                className="group backdrop-blur-xl bg-white/5 border border-white/10 hover:border-indigo-500/50 rounded-xl p-6 transition transform hover:scale-105"
              >
                <h3 className="text-xl font-bold text-white group-hover:text-indigo-400 transition mb-2">
                  {board.name}
                </h3>
                {board.description && (
                  <p className="text-gray-400 text-sm mb-4 line-clamp-2">
                    {board.description}
                  </p>
                )}
                <p className="text-xs text-gray-500">
                  {new Date(board.createdAt).toLocaleDateString('pt-BR')}
                </p>
              </button>
            ))}
          </div>
        )}
      </main>
    </div>
  );
}

