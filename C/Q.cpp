#include <stdio.h>
#include <stdlib.h>
#include <windows.h>
#include <conio.h>

#define UP   0
#define DOWN 1
#define LEFT 2
#define RIGHT 3

#define HIGH  35
#define WIDTH 80   //必须为偶数

int temp[HIGH][WIDTH];//像素数组，用于打印图形（蛇、食物、空格）

int STATE = LEFT;//初始状态为向左走

int FOODPOS = 700; //WIDTH*HIGH/2+WIDTH/2-WIDTH*HIGH/4;//食物的初始位置

#define INIT_LEN 1  //初始蛇的长度
int init_pos[INIT_LEN] = {HIGH/2*WIDTH+WIDTH/2};//蛇的初始位置
int SCORE = 0;//得分

//蛇的结构体
struct Item{
    int pos;//位置
    int state;//状态：上下左右
    struct Item *per, *next;
};

struct Item *head, *tail, *iTemp;

char symbol = 2;//元素符号

//获取元素的位置
int getPos(int pos, int state){
    if(state == UP)
        pos -= WIDTH;
    else if(state == DOWN)
        pos += WIDTH;
    else if(state == LEFT)
        pos -= 2;
    else if(state == RIGHT)
        pos += 2;
    else
    {
        printf("\n状态state出错！");
        exit(-1);
    }
    return pos;
}

//更新蛇的位置与状态
void change(){
    while(1){
        printf();
        iTemp = tail;
        while(iTemp != head){
            iTemp->state = iTemp->per->state;//当前元素的状态等于其前驱的状态
            iTemp->pos = getPos(iTemp->pos,iTemp->state);//更新位置信息
            iTemp = iTemp->per;
        }
        head->state = STATE;
        head->pos = getPos(head->pos, head->state);
    }
}

//打印图形
void print(){
    int h, w;
    for(h = 1; h < HIGH-1; h++)//初始化像素数组
        for(w = 1; w < WIDTH-1; w++)
            temp[h][w] = 0;
    for(w = 0; w < WIDTH; w += 2){//置第0行和最后一行为2
        temp[0][w] = 2;
        temp[HIGH-1][w] = 2;
    }
    for(h = 0; h < HIGH; h++){//置第0列和最后一列为2
        temp[h][0] = 2;
        temp[h][WIDTH-1] = 2;
    }

    iTemp = head->next;
    while(iTemp){
        h = iTemp->pos / WIDTH;
        w = iTemp->pos % WIDTH;
        temp[h][w] = 1;
        iTemp = iTemp->next;
    }

    h = head->pos / WIDTH;
    w = head->pos % WIDTH;
    int i;
    if(temp[h][w] == 2 || temp[h][w] == 1){//撞到墙或自己，游戏结束head->pos < 0 || head->pos > HIGH*WIDTH
        for(i = 0; i < (WIDTH-12)/2; i++)
            printf(" ");
        printf("游戏结束:%d分", SCORE);
        getchar();
        exit(0);
    }
    else
        temp[h][w] = 1;

    temp[FOODPOS/WIDTH][FOODPOS%WIDTH] = 1;
    if(head->pos == FOODPOS){
        //吃食物
        struct Item *new = (struct Item*)malloc(sizeof(struct Item));
        new->next = head;
        head->per = new;
        //head = new;
        new->per = NULL;
        new->pos = getPos(head->pos, head->state);
        new->state = head->state;
        head = new;
        SCORE += 10;

        //产生新食物
        while(temp[FOODPOS/WIDTH][FOODPOS%WIDTH] == 1 || temp[FOODPOS/WIDTH][FOODPOS%WIDTH] == 2 || FOODPOS % 2 ==1){
            FOODPOS = rand() % (HIGH * WIDTH);
        }
        temp[FOODPOS/WIDTH][FOODPOS%WIDTH] = 1;
    }


    //清屏并打印图形
    system("cls");
    for(h = 0; h < HIGH; h++)
        for(w = 0; w < WIDTH; w++){
            if(temp[h][w] == 0)
                printf(" ");
            else if(temp[h][w] == 1)
                printf("%c", symbol);
            else if(temp[h][w] == 2)
                printf("*");
            else{
                printf("\t\t\t\t\t error!");
                exit(-2);
            }
        }
}

//初始化结构体
void init(){
    int i;

    head = (struct Item*)malloc(sizeof(struct Item));
    head->per = NULL;
    iTemp = head;
    iTemp->pos = init_pos[0];
    iTemp->state = LEFT;

    for(i = 1; i < INIT_LEN; i++){
        iTemp->next = (struct Item*)malloc(sizeof(struct Item));
        iTemp->next->per = iTemp;
        iTemp = iTemp->next;
        iTemp->pos = init_pos[i];
        iTemp->state = LEFT;
    }

    iTemp->next = NULL;
    tail = iTemp;
}

DWORD WINAPI Thread1(LPVOID pM){
    while(1){
        change();
    }
    return 0;
}

DWORD WINAPI Thread2(LPVOID pM){
    char c;
    while(1){
        c = _getch();//控制蛇的行进方向
        if(c == 'a' && STATE != RIGHT)
            STATE = LEFT;
        else if(c == 'd' && STATE != LEFT)
            STATE = RIGHT;
        else if(c == 'w' && STATE != DOWN)
            STATE = UP;
        else if(c == 's' && STATE != UP)
            STATE = DOWN;
        else
            continue;
    }
    return 0;
}

int main(void) {
    system("mode con cols=80 lines=40");//设置窗口大小
    system("color 8f");//设置窗口颜色
    init();

    HANDLE handle1 = CreateThread(NULL, 0, Thread1, NULL, 0, NULL);//线程1
    HANDLE handle2 = CreateThread(NULL, 0, Thread2, NULL, 0, NULL);//线程2

    WaitForSingleObject(handle1, INFINITE);
    WaitForSingleObject(handle2, INFINITE);
    return 0;
}