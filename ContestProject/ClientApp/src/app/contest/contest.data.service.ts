import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UserTaskCode } from './user-task-code';

@Injectable()
export class ContestDataService {

    private url = "/api/contest";

    constructor(private http: HttpClient) {
    }

    getTaskNames() {
        return this.http.get(this.url);
    }

    getContestTask(taskName: string) {
        return this.http.get(this.url + '/' + taskName);
    }

    addUserTask(userTaskCode: UserTaskCode) {
        return this.http.post(this.url, userTaskCode);
    }
}