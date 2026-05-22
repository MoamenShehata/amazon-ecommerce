import {Component} from "@angular/core";
import {AppServicesProvider} from "../../../core/services/app-services.provider";
import {OAuthService} from "angular-oauth2-oidc";
import {authConfig} from "../../constants/oidc-config";

@Component({
  selector: "app-sign-in",
  standalone: true,
  imports: [],
  templateUrl: "./sign-in.component.html",
})
export class SignInComponent extends AppServicesProvider {
  constructor(private oauthService: OAuthService) {
    super();
    // this.authService.configure();
  }

  ngOnInit() {
    this.authService.initiateCodeFlow();
    // if (!this.oauthService.hasValidAccessToken()) {
    //   this.oauthService.initLoginFlow();
    // }
    // if (this.activatedRoute.snapshot.url[0].path.includes("signin")) {
    //   this.oauthService.initLoginFlow();
    // }
    // this.oauthService.tryLoginCodeFlow().then(() => {});
    // this.oauthService.configure(authConfig);
    // this.oauthService.loadDiscoveryDocument().then((event) => {
    //   this.oauthService.initCodeFlow();
    //   // this.oauthService.tryLoginCodeFlow();
    // });
  }
}
