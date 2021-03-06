import { Component, OnInit } from '@angular/core';
import { ContestDataService } from './contest.data.service';
import { UserTaskCode } from './user-task-code';
import { CookieService } from 'ngx-cookie-service';

@Component({
    templateUrl: './contest.component.html',
    styleUrls: ['./contest.component.css']
})
export class ContestComponent implements OnInit {

    public initCode: string = "public static int MyMethod(int input) {\nreturn 1;\n}";
    public userTaskCode: UserTaskCode;
    public description: string;
    public taskNames: string[];
    public info: string;
    public isRunned: boolean = false;
    public runTouched: boolean = false;


    constructor(private dataService: ContestDataService,
        private cookieService: CookieService) {
    }

    ngOnInit() {
        this.dataService.getTaskNames().subscribe((data: string[]) => {
            this.taskNames = data;
            this.userTaskCode = { userName: "", taskName: data[0], code: this.initCode };

            if (this.cookieService.check('userName')) {
                this.userTaskCode.userName = this.cookieService.get('userName');
            }
            if (this.cookieService.check('taskName')) {
                this.userTaskCode.taskName = this.cookieService.get('taskName');
            }

            this.dataService.getContestTask(this.userTaskCode.taskName).subscribe((data: string) => this.description = data);
        });
    }

    chooseTask() {
        this.cookieService.set('taskName', this.userTaskCode.taskName);
        this.dataService.getContestTask(this.userTaskCode.taskName).subscribe((data: string) => this.description = data);
    }

    saveResult() {
        this.runTouched = true;
        if (this.userTaskCode.userName == "") return;
        this.cookieService.set('userName', this.userTaskCode.userName);
        this.isRunned = true;
        this.info = "WAIT...";
        this.dataService.addUserTask(this.userTaskCode).subscribe((data: string) => { this.info = data });
    }


}