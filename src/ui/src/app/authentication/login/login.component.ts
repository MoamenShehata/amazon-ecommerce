import { Component } from '@angular/core';
import { AuthService } from '../services/authentication.service';
import { AppServicesProvider } from '../../core/services/app-services.provider';
import { StorageService } from '../../core/services/storage-service';
import { StorageKeys } from '../../core/services/constants/storage-keys';

@Component({
  selector: 'login',
  standalone: true,
  imports: [],
  template: ``,
})
export class LoginComponent extends AppServicesProvider {
  constructor(private storageService: StorageService) {
    super();
  }

  ngOnInit() {
    this.activatedRoute.queryParams.subscribe((params) => {
      let returnUrlFromState = params['state'].split(';')[1];
      if (returnUrlFromState)
        this.storageService.save(
          StorageKeys.returnUrl,
          decodeURIComponent(returnUrlFromState),
        );
    });

    this.authService.processCodeFlowCallback();
  }
}
