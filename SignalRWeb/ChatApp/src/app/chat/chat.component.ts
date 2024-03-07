 import { Component, EventEmitter, NgModuleRef, OnDestroy, OnInit, Output } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ChatService } from 'src/service/chat.service';
import { PrivatechatComponent } from '../privatechat/privatechat.component';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css']
})
export class ChatComponent implements OnInit, OnDestroy {
 @Output() closeChatEmitter = new EventEmitter();
  constructor(public chatService:ChatService, private modelService:NgbModal) { }

 ngOnInit(): void {
    this.chatService.createChatConnection();
  }
  ngOnDestroy(): void {
    this.chatService.stopChatConnection();
  } 

  backToHome(){
      this.closeChatEmitter.emit();
  }
 sendMessage(content:string){
  this.chatService.sendMessage(content);
 }

 openPrivateChat(toUser : string){

  const modalRef = this.modelService.open(PrivatechatComponent);
  modalRef.componentInstance.toUser = toUser;
  
 }

}
