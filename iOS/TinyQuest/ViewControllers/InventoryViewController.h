//
//  InventoryViewController.h
//  TinyQuest
//
//  Created by Daigo Sato on 12/4/11.
//  Copyright (c) 2011 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface InventoryViewController : NSObject
{
}

- (BOOL)setup;
- (void)setupInventory;

@property(strong, nonatomic) IBOutlet UIScrollView *scrollView;
@end
