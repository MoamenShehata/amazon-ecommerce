import { RoleNames } from './role-names';

export class AuthenticatedUser {
  constructor(
    public id: number,
    public name: string,
    public email: string,
    public role: string,
  ) {}

  get isAdmin() {
    return this.role == RoleNames.Admin;
  }
}
