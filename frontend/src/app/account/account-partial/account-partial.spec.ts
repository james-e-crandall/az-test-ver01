import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AccountPartial } from './account-partial';

describe('AccountPartial', () => {
  let component: AccountPartial;
  let fixture: ComponentFixture<AccountPartial>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AccountPartial],
    }).compileComponents();

    fixture = TestBed.createComponent(AccountPartial);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
