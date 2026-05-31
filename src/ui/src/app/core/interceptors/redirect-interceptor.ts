// import { HttpInterceptor, HttpResponse } from "@angular/common/http";
// import { tap } from "rxjs";
// import { AppServicesProvider } from "../services/app-services.provider";

// export class RedirectInterceptor extends AppServicesProvider implements HttpInterceptor {

//     intercept(req: any, next: any) {
//         return next.handle(req).pipe(
//             tap((event) => {
//                 if (event instanceof HttpResponse) {
//                     if (event.status === 308) {
//                         const redirectUrl = event.headers.get("Location");
//                         if (redirectUrl) {
//                             this.router.navigateByUrl(redirectUrl);
//                             // window.location.href = redirectUrl;
//                         }
//                     }
//                 }
//             })
//         );
//     }
// }
