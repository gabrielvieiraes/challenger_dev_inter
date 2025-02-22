import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateOrUpdateSalaComponent } from './create-or-update-sala.component';

describe('CreateOrUpdateSalaComponent', () => {
  let component: CreateOrUpdateSalaComponent;
  let fixture: ComponentFixture<CreateOrUpdateSalaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CreateOrUpdateSalaComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreateOrUpdateSalaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
