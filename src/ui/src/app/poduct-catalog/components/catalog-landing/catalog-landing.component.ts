import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';

@Component({
  selector: 'app-catalog-landing',
  standalone: true,
  imports: [RouterModule],
  templateUrl: './catalog-landing.component.html',
  styleUrl: './catalog-landing.component.css',
})
export class CatalogLandingComponent {}
