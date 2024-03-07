import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ChatService } from 'src/service/chat.service';
import { ChatComponent } from '../chat/chat.component';

@Component({
  selector: 'app-privatechat',
  templateUrl: './privatechat.component.html',
  styleUrls: ['./privatechat.component.css']
})
export class PrivatechatComponent implements OnInit, OnDestroy {
@Input() toUser = '';
  constructor(public activeModel:NgbActiveModal, public chatService:ChatService) { }

  ngOnDestroy(): void {
    this.chatService.closePrivateChatMessage(this.toUser);
   
  }

  ngOnInit(): void {
   
  }

  sendMessage(content: string){
     this.chatService.sendPrivateMessage(this.toUser, content);
     content = '';
  }

}

