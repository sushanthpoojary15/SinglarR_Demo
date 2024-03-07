import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { PrivatechatComponent } from 'src/app/privatechat/privatechat.component';
import { environment } from 'src/environments/environment';
import { Message } from 'src/models/message';
import { User } from 'src/models/user';

@Injectable({
  providedIn: 'root'
})
export class ChatService {
  myName: string = '';
  private chatConnection?: HubConnection;
  onlineUsers: string[] = [];
  messages : Message[] = [];
  privateMessages: Message[]=[];
  privateMessageInitiated = false;


  constructor(private httpClient: HttpClient, private modelService:NgbModal) { }

  registerUser(user: User) {
    return this.httpClient.post(`${environment.apiUrl}/api/service/register-user`, user, { responseType: 'text' });
  }

  createChatConnection() {
    this.chatConnection = new HubConnectionBuilder()
      .withUrl(`${environment.apiUrl}/hubs/chat`).withAutomaticReconnect().build();

    this.chatConnection.start().catch(error => {
      console.log(error);
    });

    this.chatConnection.on('UserConnected', () => {
       this.addUserConnectionId();

    });

    this.chatConnection.on('OnlineUsers', (onlineUsers) => {
      this.onlineUsers = [...onlineUsers];
    });

    this.chatConnection.on('NewMessages', (newMessages: Message) => {
      this.messages = [...this.messages, newMessages];
    });

    this.chatConnection.on('OpenPrivateChat', (newMessages: Message) => {
      this.privateMessages = [...this.privateMessages, newMessages];
      this.privateMessageInitiated = true;
      const modalRef = this.modelService.open(PrivatechatComponent);
      modalRef.componentInstance.toUser = newMessages.from;

    });

    this.chatConnection.on('ReceivePrivateMessage', (newMessages: Message) => {
      this.privateMessages = [...this.privateMessages, newMessages];
    });

    this.chatConnection.on('ClosePrivateChat', (newMessages: Message) => {
      this.privateMessageInitiated = false;
      this.privateMessages = [];
      this.modelService.dismissAll();
    });

  }

  stopChatConnection() {
    this.chatConnection?.stop().catch(error => {
      console.log(error);
    });
  }

 
  async addUserConnectionId() {
    return this.chatConnection?.invoke('AddUserConnectionId', this.myName)
      .catch(error => console.log(error));
  }

  async sendMessage (content: string){

    const message: Message = {
      from : this.myName,
      content

    };

    return this.chatConnection?.invoke('ReceiveMessage', message)
    .catch(error => console.error(error));
    
  }

  async sendPrivateMessage(to : string, content:string){
    const message: Message = {
      from : this.myName,
      to,
      content

    };    
    if(!this.privateMessageInitiated){
      this.privateMessageInitiated = true;
      return this.chatConnection?.invoke('CreatePrivateChat', message).then(() => {
        this.privateMessages = [...this.privateMessages, message];
      })
    .catch(error => console.error(error));
    }
     else{
      return this.chatConnection?.invoke('ReceivePrivateMessage', message)
      .catch(error => console.error(error));
     }
  }
  async closePrivateChatMessage(otherUser: string){
    return this.chatConnection?.invoke('RemovePrivateChat', this.myName, otherUser)
      .catch(error => console.log(error));
  }

  }

