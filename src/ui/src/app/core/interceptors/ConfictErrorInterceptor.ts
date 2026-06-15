import { HttpInterceptor, HttpRequest, HttpHandler, HttpErrorResponse } from "@angular/common/http";
import { catchError, throwError } from "rxjs";
import { AppServicesProvider } from "../services/app-services.provider";


export class ConfictErrorInterceptor extends AppServicesProvider implements HttpInterceptor {

    intercept(req: HttpRequest<any>, next: HttpHandler) {
        return next
            .handle(req)
            .pipe(catchError((err: HttpErrorResponse, x) => {
                if (err.status == 409) {
                    this.toastError(err.error);
                }
                return throwError(() => err);
            }));
    }
}
