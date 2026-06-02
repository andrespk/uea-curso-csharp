interface ApiResponse<T> {
  data?: T;
  error?: string;
  status: number;
}

const API_BASE_URL = '/api';

async function request<T>(
  endpoint: string,
  method: 'GET' | 'POST' | 'PUT' | 'DELETE' = 'GET',
  body?: unknown
): Promise<ApiResponse<T>> {
  try {
    const token = localStorage.getItem('token');
    const headers: HeadersInit = {
      'Content-Type': 'application/json',
    };

    if (token) {
      headers.Authorization = `Bearer ${token}`;
    }

    const response = await fetch(`${API_BASE_URL}${endpoint}`, {
      method,
      headers,
      body: body ? JSON.stringify(body) : undefined,
    });

    if (!response.ok) {
      const error = await response.json();
      return {
        status: response.status,
        error: error.detail || 'Erro desconhecido',
      };
    }

    if (response.status === 204) {
      return { status: 204 };
    }

    const data = await response.json();
    return { data, status: response.status };
  } catch (error) {
    return {
      status: 0,
      error: error instanceof Error ? error.message : 'Erro de conexão',
    };
  }
}

export const authService = {
  login: (username: string, password: string) =>
    request<{ token: string }>('/auth/login', 'POST', { username, password }),
};

export default request;

