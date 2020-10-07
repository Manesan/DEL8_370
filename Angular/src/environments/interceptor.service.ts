import { Injectable } from '@angular/core';
    import {
    HttpInterceptor, HttpRequest,
    HttpHandler, HttpEvent, HttpErrorResponse
    } from '@angular/common/http';
    import { Observable, throwError } from 'rxjs';
    import { catchError } from 'rxjs/operators';
    @Injectable({
    providedIn: 'root'
    })
    export class InterceptorService implements HttpInterceptor{
     constructor() { }
     handleError(error: HttpErrorResponse){
      console.log("An error has occured");
      return throwError(error);
     }
    intercept(req: HttpRequest<any>, next: HttpHandler):
    Observable<HttpEvent<any>>{
     return next.handle(req)
     .pipe(
      catchError(this.handleError)
     )
     };
    }



//       //Assign form validation
//   async submitAssign(){
//     if(this.inspectorid != "" ){
//       //console.log(this.descriptionInput);
//       $("#assignModal").modal('hide');
//       $("#confirmCaptureAssignmentModal").modal('show');
//     }
//   }

//  //Generate report form validation
//  async submitGenerateInpsection(){
//   if(this.startDate != "" && this.endDate != ""){
//       //console.log(this.descriptionInput);
//       $("#editModal").modal('hide');
//       $("#confirmEditModal").modal('show');
//     }
//   }

//   async submitCapture(){
//     $("#captureModal").modal('hide');
//     $("#confirmCaptureModal").modal('show');
//   }
