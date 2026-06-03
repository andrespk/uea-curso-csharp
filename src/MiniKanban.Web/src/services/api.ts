export interface ApiResponse<T> {
  data?: T;
  error?: string;
  status: number;
}

export interface User {
  id: string;
  name: string;
  username: string;
  email: string;
  role: string;
  createdAt: string;
}

export interface Board {
  id: string;
  name: string;
  description?: string;
  ownerId: string;
  createdAt: string;
}

export interface BoardMember {
  id: string;
  boardId: string;
  userId: string;
  role: 'Owner' | 'Admin' | 'Member' | 'Viewer' | number;
  joinedAt: string;
}

export interface KanbanColumn {
  id: string;
  boardId: string;
  name: string;
  order: number;
  wipLimit?: number;
  createdAt: string;
}

export interface Card {
  id: string;
  columnId: string;
  createdByUserId: string;
  assignedToUserId?: string;
  title: string;
  description?: string;
  priority: number;
  createdAt: string;
  dueDate?: string;
}

export interface Tag {
  id: string;
  boardId: string;
  name: string;
  color?: string;
}

export interface CardTag {
  cardId: string;
  tagId: string;
}

export interface Comment {
  id: string;
  cardId: string;
  userId: string;
  content: string;
  createdAt: string;
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
      let message = 'Erro desconhecido';
      try {
        const error = await response.json();
        message = error.detail || error.title || message;
      } catch {
        message = await response.text();
      }

      return {
        status: response.status,
        error: message || `Erro HTTP ${response.status}`,
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
      error: error instanceof Error ? error.message : 'Erro de conexao',
    };
  }
}

export const authService = {
  login: (username: string, password: string) =>
    request<{ username: string; token: string }>('/auth/login', 'POST', { username, password }),
  register: (payload: {
    name: string;
    username: string;
    email: string;
    password: string;
    role: string;
  }) => request<User>('/auth/register', 'POST', payload),
  me: () => request<User>('/me'),
};

export const userService = {
  list: () => request<User[]>('/users'),
  getById: (id: string) => request<User>(`/users/${id}`),
};

export const boardService = {
  list: () => request<Board[]>('/boards'),
  get: (id: string) => request<Board>(`/boards/${id}`),
  listOwnedBy: (ownerId: string) => request<Board[]>(`/users/${ownerId}/boards/owned`),
  create: (payload: { name: string; description?: string }) =>
    request<Board>('/boards', 'POST', payload),
  update: (id: string, payload: { name?: string; description?: string }) =>
    request<Board>(`/boards/${id}`, 'PUT', payload),
  remove: (id: string) => request<void>(`/boards/${id}`, 'DELETE'),
};

export const boardMemberService = {
  list: (boardId: string) => request<BoardMember[]>(`/boards/${boardId}/members`),
  add: (payload: { boardId: string; userId: string; role: string }) =>
    request<BoardMember>('/board-members', 'POST', payload),
  update: (id: string, role: string) =>
    request<BoardMember>(`/board-members/${id}`, 'PUT', { role }),
  remove: (id: string) => request<void>(`/board-members/${id}`, 'DELETE'),
};

export const columnService = {
  list: (boardId: string) => request<KanbanColumn[]>(`/boards/${boardId}/kanban-columns`),
  create: (payload: { boardId: string; name: string; order: number; wipLimit?: number }) =>
    request<KanbanColumn>('/kanban-columns', 'POST', payload),
  update: (id: string, payload: { name?: string; order?: number; wipLimit?: number }) =>
    request<KanbanColumn>(`/kanban-columns/${id}`, 'PUT', payload),
  remove: (id: string) => request<void>(`/kanban-columns/${id}`, 'DELETE'),
};

export const cardService = {
  listByColumn: (columnId: string) => request<Card[]>(`/columns/${columnId}/cards`),
  create: (payload: {
    columnId: string;
    assignedToUserId?: string;
    title: string;
    description?: string;
    priority: number;
    dueDate?: string;
  }) => request<Card>('/cards', 'POST', payload),
  update: (id: string, payload: Partial<Card>) => request<Card>(`/cards/${id}`, 'PUT', payload),
  remove: (id: string) => request<void>(`/cards/${id}`, 'DELETE'),
};

export const tagService = {
  list: (boardId: string) => request<Tag[]>(`/boards/${boardId}/tags`),
  create: (payload: { boardId: string; name: string; color?: string }) =>
    request<Tag>('/tags', 'POST', payload),
  update: (id: string, payload: { name?: string; color?: string }) =>
    request<Tag>(`/tags/${id}`, 'PUT', payload),
  remove: (id: string) => request<void>(`/tags/${id}`, 'DELETE'),
};

export const cardTagService = {
  list: (cardId: string) => request<CardTag[]>(`/cards/${cardId}/tags`),
  add: (payload: { cardId: string; tagId: string }) =>
    request<CardTag>('/card-tags', 'POST', payload),
  remove: (cardId: string, tagId: string) =>
    request<void>(`/cards/${cardId}/tags/${tagId}`, 'DELETE'),
};

export const commentService = {
  list: (cardId: string) => request<Comment[]>(`/cards/${cardId}/comments`),
  create: (payload: { cardId: string; content: string }) =>
    request<Comment>('/comments', 'POST', payload),
  update: (id: string, content: string) => request<Comment>(`/comments/${id}`, 'PUT', { content }),
  remove: (id: string) => request<void>(`/comments/${id}`, 'DELETE'),
};

export default request;
