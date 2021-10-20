import { Component, OnInit } from '@angular/core';
import { ContestDataService } from './contest.data.service';
import { UserTaskCode } from './user-task-code';
import { ContestTask } from './contest-task';

@Component({
    templateUrl: './contest.component.html'
})
export class ContestComponent implements OnInit {

    public userTaskCode: UserTaskCode = { name: "", task: "", code: "", inputParameter: 0, outputParameter: 0 };
    public contestTask: ContestTask = { task: "", description: "", inputParameter: 0, outputParameter: 0 };
    public taskNames: string[];
    public userName: string = "";
    public selectedTask: string = "TheSimpliestTask";
    public code: string = "public static int MyMethod(int input) {\nreturn 5 * input;\n}";
    public info: string;
    public isRunned: boolean = false;


    constructor(private dataService: ContestDataService) {
    }

    ngOnInit() {
        this.dataService.getTaskNames().subscribe((data: string[]) => this.taskNames = data);
        this.dataService.getContestTask(this.selectedTask).subscribe((data: ContestTask) => this.contestTask = data);
    }

    chooseTask() {
        this.dataService.getContestTask(this.selectedTask).subscribe((data: ContestTask) => this.contestTask = data);
    }

    saveResult() {
        this.isRunned = true;
        this.info = "WAIT...";
        this.userTaskCode = { name: this.userName, task: this.selectedTask, code: this.code, inputParameter: this.contestTask.inputParameter, outputParameter: this.contestTask.outputParameter };
        this.dataService.addUserTask(this.userTaskCode).subscribe((data: boolean) => {
            if (data) {
                this.info = "!!!SUCCESS!!!"
            }
            else {
                this.info = "WRONG :("
            }
        });
    }
}