import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { Routes, RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { LeaderboardComponent } from './leaderboard/leaderboard.component';
import { ContestComponent } from './contest/contest.component';
import { NotFoundComponent } from './not-found.component';

import { ContestDataService } from './contest/contest.data.service';
import { LeaderboardDataService } from './leaderboard/leaderboard.data.service'

const appRoutes: Routes = [
    { path: 'leaderboard', component: LeaderboardComponent },
    { path: '', component: ContestComponent },
    { path: '**', component: NotFoundComponent }
];

@NgModule({
    imports: [BrowserModule, FormsModule, HttpClientModule, RouterModule.forRoot(appRoutes)],
    declarations: [AppComponent, LeaderboardComponent, ContestComponent, NavMenuComponent, NotFoundComponent],
    providers: [ContestDataService, LeaderboardDataService],
    bootstrap: [AppComponent]
})
export class AppModule { }