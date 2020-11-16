import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TagListComponent } from './tag-list/tag-list.component';
import { RouterModule } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';

@NgModule({
  declarations: [TagListComponent],
  imports: [
    //vendor
    CommonModule,
    RouterModule,

    //material
    MatCardModule,
    MatButtonModule
  ],
  exports: [
    //vendor
    CommonModule,
    RouterModule,

    //material
    MatCardModule,
    MatButtonModule,

    //local
    TagListComponent
  ]
})
export class SharedModule { }
