export interface LoginDto {
  email: string;
  password: string;
}

export interface RegisterDto {
  fullName: string;
  email: string;
  password: string;
  role: string;
  phoneNumber?: string;
}

export interface UserToken {
  token: string;
  userId: number;
  fullName: string;
  email: string;
  role: string;
}
