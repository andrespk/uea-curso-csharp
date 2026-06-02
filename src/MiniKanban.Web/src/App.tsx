import { useState, useEffect } from 'react';
import { LoginPage } from './components/LoginPage';
import { Dashboard } from './components/Dashboard';
import { BoardView } from './components/BoardView';
import './index.css';

type Page = 'login' | 'dashboard' | 'board';

function App() {
  const [page, setPage] = useState<Page>('login');
  const [selectedBoardId, setSelectedBoardId] = useState<string>('');
  const [token, setToken] = useState<string>('');

  useEffect(() => {
    const storedToken = localStorage.getItem('token');
    if (storedToken) {
      setToken(storedToken);
      setPage('dashboard');
    }
  }, []);

  const handleLoginSuccess = (newToken: string) => {
    setToken(newToken);
    setPage('dashboard');
  };

  const handleLogout = () => {
    localStorage.removeItem('token');
    setToken('');
    setPage('login');
  };

  const handleBoardSelect = (boardId: string) => {
    setSelectedBoardId(boardId);
    setPage('board');
  };

  const handleBackToDashboard = () => {
    setPage('dashboard');
  };

  return (
    <>
      {page === 'login' && <LoginPage onLoginSuccess={handleLoginSuccess} />}
      {page === 'dashboard' && (
        <Dashboard onBoardSelect={handleBoardSelect} onLogout={handleLogout} />
      )}
      {page === 'board' && (
        <BoardView boardId={selectedBoardId} onBack={handleBackToDashboard} />
      )}
    </>
  );
}

export default App;

