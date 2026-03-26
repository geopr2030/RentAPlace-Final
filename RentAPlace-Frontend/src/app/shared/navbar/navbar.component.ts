import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { MessageService, MessageResponseDto } from '../../services/message.service';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent implements OnInit, OnDestroy {
  unreadMessages: MessageResponseDto[] = [];
  showNotifications = false;
  private pollInterval: any;

  constructor(public auth: AuthService, private messageService: MessageService) {}

  ngOnInit() {
    if (this.auth.isLoggedIn()) {
      this.fetchNotifications();
      // Poll every 15 seconds
      this.pollInterval = setInterval(() => {
        this.fetchNotifications();
      }, 15000);
    }
  }

  ngOnDestroy() {
    if (this.pollInterval) clearInterval(this.pollInterval);
  }

  fetchNotifications() {
    if (!this.auth.isLoggedIn()) return;
    this.messageService.getInbox().subscribe({
      next: (msgs) => {
        this.unreadMessages = msgs.filter(m => !m.isRead).sort((a,b) => new Date(b.sentAt).getTime() - new Date(a.sentAt).getTime());
      },
      error: (err) => console.error('Error fetching notifications', err)
    });
  }

  toggleNotifications() {
    this.showNotifications = !this.showNotifications;
  }

  markAsRead(msg: MessageResponseDto) {
    this.messageService.markAsRead(msg.messageId).subscribe({
      next: () => {
        msg.isRead = true;
        this.unreadMessages = this.unreadMessages.filter(m => !m.isRead);
      }
    });
  }
}
