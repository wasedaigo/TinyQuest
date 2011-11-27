//
//  ItemDialogController.h
//  TinyQuest
//
//  Created by Daigo Sato on 11/27/11.
//  Copyright (c) 2011 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface ItemDialogController : NSObject
{
}
- (BOOL)loadNib;
- (void)slideIn;
- (void)slideOut;
- (IBAction)backButtonClicked:(id)sender;
@property(weak, nonatomic) IBOutlet UIView *view;
@end
