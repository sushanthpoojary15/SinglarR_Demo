import { Component, EventEmitter, OnInit, Output, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-chat-input',
  templateUrl: './chat-input.component.html',
  styleUrls: ['./chat-input.component.css']
})
export class ChatInputComponent implements OnInit {
@Output() contentEmitter = new EventEmitter();
@ViewChild('messageFrom') messageFrom: NgForm | undefined;
  content: string = '';
  constructor() { }

  ngOnInit(): void {
  }
  sendMessage(){
    if(this.content.trim() != ""){
      this.contentEmitter.emit(this.content);
      this.content = '';
    }

  }

}
