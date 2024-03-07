import { Component, Input, OnInit, Output } from '@angular/core';
import { Message } from 'src/models/message';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.css']
})
export class MessagesComponent implements OnInit {
@Input() messages: Message[] = [];
  constructor() { }

  ngOnInit(): void {

    console.log(this.messages);
    
  }

}
