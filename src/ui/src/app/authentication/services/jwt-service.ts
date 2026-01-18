import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { StorageService } from '../../../core/services/storage-service';
import { StorageKeys } from '../../../core/constants/storage-keys';
import { AuthenticatedUser } from '../models/authenticated-user.model';
import { UserClaimTypes } from '../constants/custom-claim.constants';

@Injectable({
  providedIn: 'root',
})
export class JwtService {
  constructor(
    private jwtService: JwtHelperService,
    private storageService: StorageService
  ) {}

  // constructUserFromToken(id_token: any) {
  //   let decodedToken = this.jwtService.decodeToken(id_token);

  //   let user = new AuthenticatedUser(
  //     id_token,
  //     decodedToken.sub,
  //     decodedToken.name,
  //     decodedToken.email,
  //     decodedToken[
  //       'http://schemas.microsoft.com/ws/2008/06/identity/claims/role'
  //     ],
  //     decodedToken.isActive == 'True',
  //     decodedToken[UserClaimTypes.AuthorSubscriptionId]
  //   );

  //   return user;
  // }

  tryGetSavedToken() {
    const token = this.storageService.retrieve(StorageKeys.userToken);

    if (this.jwtService.isTokenExpired(token)) return null;

    return token;
  }
}
