//
//  ViewController.h
//  TinyQuest
//
//  Created by sato.daigo on 11/11/26.
//  Copyright (c) 2011年 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>

@class ItemDialogController;
@interface ViewController : UIViewController
{
    IBOutlet UIScrollView *scrollView;
    ItemDialogController *itemDialogController;
}
@property(strong) ItemDialogController *itemDialogController;
@end

