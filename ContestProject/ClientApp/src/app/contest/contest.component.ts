import { Component, OnInit } from '@angular/core';
import { ContestDataService } from './contest.data.service';
import { UserTaskCode } from './user-task-code';

@Component({
    templateUrl: './contest.component.html'
})
export class ContestComponent implements OnInit {

    public initCode: string = "public static int MyMethod(int input) {\nreturn 1;\n}";
    public userTaskCode: UserTaskCode;
    public description: string;
    public taskNames: string[];
    public info: string;
    public isRunned: boolean = false;
    public runTouched: boolean = false;


    constructor(private dataService: ContestDataService) {
    }

    ngOnInit() {
        this.dataService.getTaskNames().subscribe((data: string[]) => {
            this.taskNames = data;
            this.userTaskCode = { userName: "", taskName: data[0], code: this.initCode };
            this.dataService.getContestTask(this.userTaskCode.taskName).subscribe((data: string) => this.description = data);
        });
    }

    chooseTask() {
        this.dataService.getContestTask(this.userTaskCode.taskName).subscribe((data: string) => this.description = data);
    }

    saveResult() {
        this.runTouched = true;
        if (this.userTaskCode.userName == "") return;
        this.isRunned = true;
        this.info = "WAIT...";
        this.dataService.addUserTask(this.userTaskCode).subscribe((data: string) => { this.info = data });
    }


}