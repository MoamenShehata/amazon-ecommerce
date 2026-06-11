// src/app/services/signalr.service.ts
import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { environment } from '../../../environments/environment';
import { AuthService } from '../../authentication/services/authentication.service';

@Injectable({
    providedIn: 'root',
})
export class SignalRService {
    private hubConnection!: signalR.HubConnection;

    constructor(private authService: AuthService) { }

    public startConnection() {
        this.hubConnection = new signalR.HubConnectionBuilder()
            .withUrl(`${environment.notificationBaseUrl}/notificationHub`, {
                accessTokenFactory: () => this.authService.accessToken,
                withCredentials: true,
            })
            .withAutomaticReconnect()
            .build();

        this.hubConnection
            .start()
            .then(() => console.log('SignalR connection started'))
            .catch((err) =>
                console.error('Error while starting SignalR connection: ', err)
            );
    }

    public addReceiveMessageListener(
        messageName: string,
        callback: (...args: any[]) => void
    ) {
        this.hubConnection.on(messageName, callback);
    }

    public sendMessage(user: string, message: string) {
        this.hubConnection
            .invoke('SendMessage', user, message)
            .catch((err) => console.error(err));
    }
}
