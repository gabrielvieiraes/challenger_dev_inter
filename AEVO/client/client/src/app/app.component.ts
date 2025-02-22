import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { FooterComponent } from './components/footer/footer.component';
import { NavbarComponent } from './components/navbar/navbar.component';
import { FlexLayoutServerModule } from '@angular/flex-layout/server';
import { TranslateService } from '@ngx-translate/core';
import translationsPTBR from '../../public/i18n/pt-BR.json';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-root',
  imports: [
    RouterOutlet,
    FooterComponent,
    NavbarComponent,
    FlexLayoutServerModule,
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent {
  title = 'AEVO Challenger';

  constructor(
    private translate: TranslateService,
    private titleService: Title
  ) {
    this.translate.addLangs(['pt-BR', 'en-US']);
    this.translate.setDefaultLang('pt-BR');
    this.translate.setTranslation('pt-BR', translationsPTBR);
    this.translate.use('pt-BR');
  }

  ngOnInit() {
    this.titleService.setTitle(this.title);
  }
}
