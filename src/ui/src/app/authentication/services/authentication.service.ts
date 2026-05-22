import {Injectable} from "@angular/core";
import {BehaviorSubject, Observable, Subject} from "rxjs";
import {OAuthService} from "angular-oauth2-oidc";
import {AuthenticatedUser} from "../models/authenticated-user.model";
import {authConfig} from "../constants/oidc-config";
import {UserClaimTypes} from "../constants/custom-claim.constants";
import {StorageService} from "../../core/services/storage-service";
import {ActivatedRoute, Router} from "@angular/router";

@Injectable({
  providedIn: "root",
})
export class AuthService {
  userSubject = new BehaviorSubject<AuthenticatedUser | null>(null);

  userLoginEvent = new Subject<AuthenticatedUser>();

  constructor(
    private storageService: StorageService,
    private oauthService: OAuthService,
    private router: Router,
    private activatedRoute: ActivatedRoute,
  ) {
    this.setAuthenticatedUser();
  }

  configure() {
    this.oauthService.configure(authConfig);

    this.oauthService.loadDiscoveryDocument().then((event) => {
      if (this.oauthService.hasValidAccessToken()) {
        this.oauthService.setupAutomaticSilentRefresh();
      }
      // else {
      //   this.oauthService.initLoginFlow();
      // }
    });
  }

  setAuthenticatedUser(triggerLoginEvent = false) {
    let userClaims = this.oauthService.getIdentityClaims();

    if (!userClaims) return;

    let user = new AuthenticatedUser(
      userClaims[UserClaimTypes.sub],
      userClaims[UserClaimTypes.name],
      userClaims[UserClaimTypes.email],
      userClaims[UserClaimTypes.role],
    );

    this.userSubject.next(user);

    if (triggerLoginEvent) this.userLoginEvent.next(user);
  }

  initiateCodeFlow(state: string = "") {
    this.oauthService.initCodeFlow(state);
  }

  processCodeFlowCallback() {
    debugger;
    this.oauthService.tryLoginCodeFlow().then(() => {
      debugger;
      if (this.oauthService.hasValidAccessToken()) {
        this.setAuthenticatedUser(true);
        const returnUrl = decodeURIComponent(this.oauthService.state ?? "/");

        this.router.navigate([returnUrl]);
      }
    });
  }

  logoutCurrentUser() {
    this.oauthService.logOut();

    this.storageService.clear();

    this.userSubject.next(null);
  }

  refreshToken() {
    this.oauthService.silentRefresh().then((event) => {
      if (this.oauthService.hasValidAccessToken()) {
        this.storageService.clear();
        this.setAuthenticatedUser();
      }
    });
  }

  getAuthenticatedUser() {
    return this.userSubject.value;
  }

  get accessToken() {
    return this.oauthService.getAccessToken();
  }

  get isAuthenticated() {
    return this.userSubject.value !== null;
  }
}
