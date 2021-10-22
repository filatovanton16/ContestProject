import { Component, OnInit } from '@angular/core';
import { LeaderboardDataService } from './leaderboard.data.service';
import { UserTaskGroup } from './user-task-group';

@Component({
    templateUrl: './leaderboard.component.html',
    styleUrls: ['./leaderboard.component.css']
})
export class LeaderboardComponent implements OnInit{
    public userTaskGroups: UserTaskGroup[];

    constructor(private dataService: LeaderboardDataService) { }

    ngOnInit() {
        this.load();
    }
    load() {
        this.dataService.getLeaderboard().subscribe((data: UserTaskGroup[]) => this.userTaskGroups = data);
    }
}
