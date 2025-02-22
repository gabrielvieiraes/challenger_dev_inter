import {
  Component,
  Inject,
  inject,
  OnDestroy,
  OnInit,
  PLATFORM_ID,
  signal,
} from '@angular/core';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MenuItem } from '../../models/menu-item';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatButton } from '@angular/material/button';
import { MatDividerModule } from '@angular/material/divider';
import { CommonModule, isPlatformBrowser } from '@angular/common';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatListModule } from '@angular/material/list';
import { MediaMatcher } from '@angular/cdk/layout';
import { NavigationEnd, Router } from '@angular/router';
import { LocaleSwitcherComponent } from '../locale-switcher/locale-switcher.component';
import { TranslatePipe } from '@ngx-translate/core';
import { filter, Subscription } from 'rxjs';

@Component({
  selector: 'app-navbar',
  imports: [
    CommonModule,
    MatToolbarModule,
    FlexLayoutModule,
    MatMenuModule,
    MatButton,
    MatIconModule,
    MatDividerModule,
    MatSidenavModule,
    MatListModule,
    LocaleSwitcherComponent,
    TranslatePipe,
  ],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css',
})
export class NavbarComponent implements OnInit, OnDestroy {
  protected readonly isMobile = signal(true);
  private readonly _mobileQuery: MediaQueryList;
  private readonly _mobileQueryListener: () => void;
  private routerSubscription: Subscription | null = null;

  menuItems: MenuItem[] = [
    {
      label: 'HOME',
      icon: 'home',
      path: '',
    },
    {
      label: 'LOGIN',
      icon: 'account_circle',
      path: '/login',
    },
  ];

  constructor(
    private router: Router,
    @Inject(PLATFORM_ID) private platformId: Object
  ) {
    const media = inject(MediaMatcher);

    this._mobileQuery = media.matchMedia('(max-width: 600px)');
    this.isMobile.set(this._mobileQuery.matches);
    this._mobileQueryListener = () =>
      this.isMobile.set(this._mobileQuery.matches);
    if (this._mobileQuery.addEventListener) {
      this._mobileQuery.addEventListener('change', this._mobileQueryListener);
    }
  }

  ngOnDestroy(): void {
    if (this._mobileQuery.removeEventListener)
      this._mobileQuery.removeEventListener(
        'change',
        this._mobileQueryListener
      );

    if (this.routerSubscription) {
      this.routerSubscription.unsubscribe();
    }
  }

  ngOnInit(): void {
    this.routerSubscription = this.router.events
      .pipe(filter((event) => event instanceof NavigationEnd))
      .subscribe((event: NavigationEnd) => {
        this.handleMenuItens();
      });
  }

  handleMenuItens() {
    if (isPlatformBrowser(this.platformId)) {
      const accessToken = localStorage.getItem('accessToken');

      if (accessToken) {
        this.menuItems = [
          {
            label: 'AGENDAMENTOS',
            icon: 'bookmark',
            path: '/agendamentos',
          },
          {
            label: 'SALAS',
            icon: 'question_answer',
            path: '/salas',
          },
          {
            label: 'SAIR',
            icon: 'exit_to_app',
            path: '/login',
          },
        ];
      } else {
        this.menuItems = [
          {
            label: 'HOME',
            icon: 'home',
            path: '',
          },
          {
            label: 'LOGIN',
            icon: 'account_circle',
            path: '/login',
          },
        ];
      }
    }
  }

  redirect(path: string) {
    if (path === '/login') {
      if (isPlatformBrowser(this.platformId)) {
        localStorage.removeItem('accessToken');
        this.menuItems = [
          {
            label: 'HOME',
            icon: 'home',
            path: '',
          },
          {
            label: 'LOGIN',
            icon: 'account_circle',
            path: '/login',
          },
        ];
      }
    }
    this.router.navigateByUrl(path);
  }
}
