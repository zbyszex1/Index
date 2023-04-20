import { Observable, Subject } from 'rxjs';
import { Injectable } from '@angular/core';
@Injectable({ providedIn: 'root' })
export class MessageService {
    private subject = new Subject<string>();
 
    sendMessage(message: string) {
        this.subject.next(message);
    }
 
    clearMessages() {
        this.subject.next('');
    }
 
    getMessage(): Observable<string> {
        return this.subject.asObservable();
    }
}