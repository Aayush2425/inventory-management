export interface RegisterRequest {
  username: string;
  password: string;
  fullname?: string;
  role?: string;
}
export interface LoginRequest {
  username: string;
  password: string;
}

export interface AuthResponse {
  data: {
    accessToken: string;
    refreshToken: string;
  };
}
