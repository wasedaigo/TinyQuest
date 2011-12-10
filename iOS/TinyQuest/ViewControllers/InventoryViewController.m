//
//  InventoryViewController.m
//  TinyQuest
//
//  Created by Daigo Sato on 12/4/11.
//  Copyright (c) 2011 __MyCompanyName__. All rights reserved.
//

#import "InventoryViewController.h"

@implementation InventoryViewController
@synthesize scrollView;
- (id)init {
    self = [super init];
    if (self != nil)
    {
        [self setup];
    }
        
    
    return self;
}

- (BOOL)setup 
{
    self.scrollView = [[UIScrollView alloc] initWithFrame:CGRectMake(0, 0, 200, 160)];
    self.scrollView.pagingEnabled = YES;

    return YES;
}

- (void)setupInventory
{
    NSInteger count = 5;
    CGSize frameSize = self.scrollView.frame.size;
    self.scrollView.contentSize=CGSizeMake(frameSize.width, frameSize.height * count);
    self.scrollView.contentInset=UIEdgeInsetsMake(0.0,0.0,0.0,0.0);
    self.scrollView.delaysContentTouches = YES;
    
    // Setup all item buttons
    UIImage *image = [UIImage imageNamed:@"slots.png"];
    for (NSInteger i = 0; i < count; i++) 
    {
        UIImageView *imageView = [[UIImageView alloc] initWithImage:image];
        imageView.frame = CGRectMake(0,  i * frameSize.height, frameSize.width, frameSize.height);
        imageView.userInteractionEnabled = YES;
        for (NSInteger buttonIndex = 0; buttonIndex < 9; buttonIndex++) {
            NSInteger tx = buttonIndex % 3;
            NSInteger ty = buttonIndex / 3;
            
            UIButton *button = [UIButton buttonWithType:UIButtonTypeCustom];
            //[button addTarget:self action:@selector(gotoProfile) forControlEvents:UIControlEventTouchUpInside];
            UIImage *image = [UIImage imageNamed:@"item1.png"];
            [button setImage:image forState:UIControlStateNormal];
            [button setFrame:CGRectMake(5 + tx * 62, 5 + ty * 54, 55, 45)];
            [imageView addSubview:button];
        }
        
        [self.scrollView addSubview:imageView];  
    }
}
@end
