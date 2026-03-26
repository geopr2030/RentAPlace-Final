import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MessageService, MessageResponseDto } from '../../services/message.service';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-inbox',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './inbox.html',
  styleUrls: ['./inbox.css']
})
export class InboxComponent implements OnInit, OnDestroy {
  messages: MessageResponseDto[] = [];
  loading = true;
  
  replyText: { [key: number]: string } = {};
  sendingReply: { [key: number]: boolean } = {};
  activeReplyId: number | null = null;
  myId: number = 0;
  private pollInterval: any;

  constructor(private messageService: MessageService, public auth: AuthService) {}

  ngOnInit() {
    this.myId = this.auth.getUserId();
    this.fetchMessages(true);

    // Auto-refresh inbox every 10 seconds for real-time feel
    this.pollInterval = setInterval(() => {
      this.fetchMessages(false);
    }, 10000);
  }

  ngOnDestroy() {
    if (this.pollInterval) {
      clearInterval(this.pollInterval);
    }
  }

  fetchMessages(showLoader: boolean = true) {
    if (showLoader) {
      this.loading = true;
    }
    this.messageService.getInbox().subscribe({
      next: (res) => {
        try {
          const list = res || [];
          this.messages = list.sort((a,b) => new Date(b.sentAt || 0).getTime() - new Date(a.sentAt || 0).getTime());
          this.markUnreadAsRead();
        } catch (e) {
          console.error(e);
        } finally {
          if (showLoader) {
            this.loading = false;
          }
        }
      },
      error: (err) => {
        console.error('Failed to fetch inbox', err);
        if (showLoader) {
          this.loading = false;
        }
      }
    });
  }

  markUnreadAsRead() {
    this.messages.forEach(m => {
      if (!m.isRead && m.receiverId === this.myId) {
        this.messageService.markAsRead(m.messageId).subscribe(() => m.isRead = true);
      }
    });
  }

  toggleReply(msgId: number) {
    if (this.activeReplyId === msgId) {
      this.activeReplyId = null;
    } else {
      this.activeReplyId = msgId;
    }
  }

  sendReply(msg: MessageResponseDto) {
    if (!this.replyText[msg.messageId]?.trim()) return;

    this.sendingReply[msg.messageId] = true;
    
    // If I am the sender of the original message, I am replying to the receiver.
    // If I am the receiver of the original message, I am replying to the sender.
    const targetUserId = (msg.senderId === this.myId) ? msg.receiverId : msg.senderId;

    const dto = {
      propertyId: msg.propertyId,
      receiverId: targetUserId,
      content: this.replyText[msg.messageId],
      parentMessageId: msg.messageId
    };

    this.messageService.sendMessage(dto).subscribe({
      next: () => {
        this.replyText[msg.messageId] = '';
        this.sendingReply[msg.messageId] = false;
        this.activeReplyId = null;
        this.fetchMessages();
      },
      error: (err) => {
        console.error('Failed to send reply', err);
        this.sendingReply[msg.messageId] = false;
      }
    });
  }
}
