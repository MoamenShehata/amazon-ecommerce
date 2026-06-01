import { HttpInterceptor, HttpResponse } from "@angular/common/http";
import { tap } from "rxjs";
import { AppServicesProvider } from "../services/app-services.provider";

export class InjectJwtInterceptor extends AppServicesProvider implements HttpInterceptor {

    intercept(req: any, next: any) {
        if (this.authService.isAuthenticated) {
            req = req.clone({
                setHeaders: {
                    Authorization: `Bearer ${this.authService.accessToken}`,
                },
            });
        }
        return next.handle(req);
    }
}
