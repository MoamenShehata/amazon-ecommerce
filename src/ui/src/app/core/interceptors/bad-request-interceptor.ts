import { HttpErrorResponse, HttpHandler, HttpInterceptor, HttpRequest, HttpResponse } from "@angular/common/http";
import { AppServicesProvider } from "../services/app-services.provider";
import { catchError, throwError } from "rxjs";


export class BadRequestInterceptor extends AppServicesProvider implements HttpInterceptor {

    intercept(req: HttpRequest<any>, next: HttpHandler) {
        return next
            .handle(req)
            .pipe(catchError((err: HttpErrorResponse, x) => {
                if (err.status == 400) {
                    this.toastError(err.error.message);
                }
                return throwError(() => err);
            }))
    }
}
