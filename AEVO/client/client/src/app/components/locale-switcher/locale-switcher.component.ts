import { CommonModule } from '@angular/common';
import { Component, Inject, LOCALE_ID } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { TranslatePipe, TranslateService } from '@ngx-translate/core';
import translationsEN from '../../../../public/i18n/en.json';
import translationsPTBR from '../../../../public/i18n/pt-BR.json';
import { MatSelectModule } from '@angular/material/select';

@Component({
  selector: 'app-locale-switcher',
  imports: [CommonModule, FormsModule, MatSelectModule, TranslatePipe],
  templateUrl: './locale-switcher.component.html',
  styleUrl: './locale-switcher.component.css',
})
export class LocaleSwitcherComponent {
  locales = [
    {
      code: 'pt-BR',
      name: 'PORTUGUES',
      timezone: 'Brazil Standard Time',
      flag: 'https://flagicons.lipis.dev/flags/4x3/br.svg',
    },
    {
      code: 'en-US',
      name: 'ENGLISH',
      timezone: 'Eastern Standard Time',
      flag: 'https://flagicons.lipis.dev/flags/4x3/us.svg',
    },
  ];

  constructor(
    @Inject(LOCALE_ID) public activeLocale: string,
    private translate: TranslateService
  ) {
    this.activeLocale = 'pt-BR';
    this.translate.setDefaultLang('pt-BR');
  }

  ngOnInit() {
    const idioma = localStorage.getItem('idioma');

    if (idioma) {
      this.activeLocale = idioma;
      this.translate.setDefaultLang(idioma);
      this.handleChangeLanguage(idioma);
    }
  }

  handleChangeLanguage(code: string) {
    switch (code) {
      case 'en-US':
        this.translate.setTranslation(code, translationsEN);
        break;
      default:
        this.translate.setTranslation(code, translationsPTBR);
        break;
    }

    localStorage.setItem('idioma', code);
    this.activeLocale = code;
    this.translate.setDefaultLang(code);
    this.translate.use(code);
  }
}
