//
//  ViewController.h
//  TinyQuest
//
//  Created by sato.daigo on 11/11/26.
//  Copyright (c) 2011年 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>
#import <GLKit/GLKit.h>

@class InventoryViewController;
@class ItemDialogController;
@interface AdventureViewController : UIViewController
{
    ItemDialogController *itemDialogController;
    InventoryViewController *inventoryViewController;
}
- (void)slideIn;
- (void)slideOut;
- (IBAction)gotoProfile;

@property(strong, nonatomic) ItemDialogController *itemDialogController;
@property(strong, nonatomic) InventoryViewController *inventoryViewController;
@property(weak, nonatomic) IBOutlet UIView *controlPanelView;
@property(weak, nonatomic) IBOutlet UIView *inventoryPanelView;
@end

