import { Token } from './user-token';

describe('jwtToken', () => {
  it('should create an instance', () => {
    expect(new Token()).toBeTruthy();
  });
});
