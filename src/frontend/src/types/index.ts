export interface User {
  id: string;
  username: string;
  displayName: string;
  role: 'admin' | 'user';
  createdAt: string;
}

export interface Media {
  id: string;
  filenameOriginal: string;
  mediaType: 'photo' | 'video' | 'document' | 'other';
  mimeType: string;
  fileSize: number;
  width?: number;
  height?: number;
  duration?: number;
  thumbnailPath?: string;
  createdAt: string;
  uploaderId: string;
}

export interface Album {
  id: string;
  name: string;
  description: string;
  coverMediaId?: string;
  ownerId: string;
  createdAt: string;
}

export interface PaginatedResponse<T> {
  items: T[];
  total: number;
  page: number;
  pageSize: number;
  totalPages: number;
}
