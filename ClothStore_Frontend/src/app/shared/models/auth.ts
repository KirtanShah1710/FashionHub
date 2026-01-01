export interface LoginDto {
  email: string;
  password: string;
}

export interface RegisterDto {
  name: string;
  email: string;
  age: number;
  password: string;
}

export interface AuthResponse {
  token: string;
}
