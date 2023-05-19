export class Token {
  jwtToken: string = '';
  refreshToken: string = '';
}

export class UserToken {
  name?: string;
  userId: number = 1;
  role?: string;
  token?: string;
  // refreshtoken?: string;
}

export class RefreshToken {
  refreshToken?: string;
  userId: number = 1;
}
