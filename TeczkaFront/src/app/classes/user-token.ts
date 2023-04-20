export class Token {
  serverToken: string = '';
}

export class UserToken {
  name?: string;
  userId: number = 1;
  role?: string;
  token?: string;
}
