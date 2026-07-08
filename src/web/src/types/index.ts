export interface User {
  id: string;
  username: string;
  displayName: string;
  role: 'admin' | 'user';
  createdAt: string;
}

export interface AuthResponse {
  token: string;
  id: string;
  username: string;
  displayName: string;
  role: string;
}
