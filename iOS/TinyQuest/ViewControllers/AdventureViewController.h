//
//  ViewController.h
//  TinyQuest
//
//  Created by sato.daigo on 11/11/26.
//  Copyright (c) 2011年 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>

@class ItemDialogController;
@interface AdventureViewController : UIViewController
{
    IBOutlet UIScrollView *scrollView;
    ItemDialogController *itemDialogController;
}
- (void)slideIn;
- (void)slideOut;
- (IBAction)gotoProfile;

@property(strong, nonatomic) ItemDialogController *itemDialogController;
@property(weak, nonatomic) IBOutlet UIView *controlPanelView;
@end

