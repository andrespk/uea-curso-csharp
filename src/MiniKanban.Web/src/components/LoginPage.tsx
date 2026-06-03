import { useState } from 'react';
import { authService } from '../services/api';

interface LoginProps {
  onLoginSuccess: (token: string) => void;
}

type AuthMode = 'login' | 'register';

export function LoginPage({ onLoginSuccess }: LoginProps) {
  const [mode, setMode] = useState<AuthMode>('login');
  const [name, setName] = useState('');
  const [username, setUsername] = useState('admin');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('Password123');
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');
  const [loading, setLoading] = useState(false);

  const fillDemoUser = () => {
    setMode('login');
    setUsername('admin');
    setPassword('Password123');
    setError('');
    setSuccess('');
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');
    setSuccess('');
    setLoading(true);

    if (mode === 'register') {
      const response = await authService.register({
        name,
        username,
        email,
        password,
        role: 'User',
      });

      if (response.error) {
        setError(response.error);
        setLoading(false);
        return;
      }

      setSuccess('Conta criada. Agora faca login com o usuario cadastrado.');
      setMode('login');
      setLoading(false);
      return;
    }

    const response = await authService.login(username, password);

    if (response.error) {
      setError(response.error);
      setLoading(false);
      return;
    }

    if (response.data?.token) {
      localStorage.setItem('token', response.data.token);
      onLoginSuccess(response.data.token);
    }

    setLoading(false);
  };

  return (
    <main className="min-h-screen bg-slate-100 text-slate-950">
      <div className="mx-auto grid min-h-screen max-w-6xl grid-cols-1 lg:grid-cols-[1fr_420px]">
        <section className="flex flex-col justify-center px-6 py-10 lg:px-10">
          <div className="max-w-2xl">
            <p className="text-sm font-semibold uppercase tracking-wider text-blue-700">
              Sistema Kanban
            </p>
            <h1 className="mt-3 text-4xl font-bold leading-tight text-slate-950 md:text-5xl">
              MiniKanban
            </h1>
            <p className="mt-5 max-w-xl text-base leading-7 text-slate-600">
              Gerencie boards, membros, colunas, cards, tags e comentarios usando a API do
              trabalho final. A interface foi feita para demonstrar os principais fluxos protegidos
              por JWT.
            </p>

            <div className="mt-8 grid max-w-xl grid-cols-1 gap-3 sm:grid-cols-3">
              <div className="rounded border border-slate-200 bg-white p-4">
                <p className="text-2xl font-bold text-slate-950">8</p>
                <p className="mt-1 text-sm text-slate-600">entidades do dominio</p>
              </div>
              <div className="rounded border border-slate-200 bg-white p-4">
                <p className="text-2xl font-bold text-slate-950">JWT</p>
                <p className="mt-1 text-sm text-slate-600">endpoints protegidos</p>
              </div>
              <div className="rounded border border-slate-200 bg-white p-4">
                <p className="text-2xl font-bold text-slate-950">EF</p>
                <p className="mt-1 text-sm text-slate-600">PostgreSQL e repositories</p>
              </div>
            </div>
          </div>
        </section>

        <section className="flex items-center px-6 py-10">
          <div className="w-full rounded border border-slate-200 bg-white p-6 shadow-sm">
            <div className="mb-6 flex rounded bg-slate-100 p-1">
              <button
                type="button"
                onClick={() => setMode('login')}
                className={`flex-1 rounded px-3 py-2 text-sm font-semibold ${
                  mode === 'login' ? 'bg-white text-slate-950 shadow-sm' : 'text-slate-600'
                }`}
              >
                Login
              </button>
              <button
                type="button"
                onClick={() => setMode('register')}
                className={`flex-1 rounded px-3 py-2 text-sm font-semibold ${
                  mode === 'register' ? 'bg-white text-slate-950 shadow-sm' : 'text-slate-600'
                }`}
              >
                Cadastro
              </button>
            </div>

            <form onSubmit={handleSubmit} className="space-y-4">
              {mode === 'register' && (
                <>
                  <TextInput label="Nome" value={name} onChange={setName} disabled={loading} />
                  <TextInput label="Email" value={email} onChange={setEmail} disabled={loading} />
                </>
              )}

              <TextInput label="Usuario" value={username} onChange={setUsername} disabled={loading} />
              <TextInput
                label="Senha"
                value={password}
                onChange={setPassword}
                disabled={loading}
                type="password"
              />

              {error && (
                <div className="rounded border border-red-200 bg-red-50 p-3 text-sm text-red-700">
                  {error}
                </div>
              )}

              {success && (
                <div className="rounded border border-emerald-200 bg-emerald-50 p-3 text-sm text-emerald-700">
                  {success}
                </div>
              )}

              <button
                type="submit"
                disabled={loading}
                className="h-11 w-full rounded bg-blue-700 px-4 text-sm font-semibold text-white hover:bg-blue-800 disabled:cursor-not-allowed disabled:opacity-60"
              >
                {loading ? 'Processando...' : mode === 'login' ? 'Entrar' : 'Criar conta'}
              </button>
            </form>

            <div className="mt-5 rounded border border-blue-100 bg-blue-50 p-4 text-sm text-blue-900">
              <div className="flex items-center justify-between gap-3">
                <div>
                  <p className="font-semibold">Usuario de demonstracao</p>
                  <p className="mt-1 font-mono text-xs">admin / Password123</p>
                </div>
                <button
                  type="button"
                  onClick={fillDemoUser}
                  className="rounded border border-blue-200 bg-white px-3 py-2 text-xs font-semibold text-blue-800 hover:bg-blue-100"
                >
                  Usar
                </button>
              </div>
            </div>
          </div>
        </section>
      </div>
    </main>
  );
}

function TextInput({
  label,
  value,
  onChange,
  disabled,
  type = 'text',
}: {
  label: string;
  value: string;
  onChange: (value: string) => void;
  disabled?: boolean;
  type?: string;
}) {
  return (
    <label className="block">
      <span className="mb-1 block text-sm font-medium text-slate-700">{label}</span>
      <input
        type={type}
        value={value}
        onChange={(e) => onChange(e.target.value)}
        disabled={disabled}
        className="h-11 w-full rounded border border-slate-300 bg-white px-3 text-sm text-slate-950 outline-none focus:border-blue-600 focus:ring-2 focus:ring-blue-100 disabled:bg-slate-100"
      />
    </label>
  );
}
